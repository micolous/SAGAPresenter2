using System;
//using GLib;
using Gtk;
using SAGAPresenter.libpresenterd;
using System.Collections.Generic;
using System.Text;

public partial class MainWindow : Gtk.Window
{
	private IPresenterd presenterd;
	private ISettings settings;
	
	// viewport which we use to contain the label and handle the scrolling of the marquee.
	private Viewport vpMarquee = new Viewport();
	
	// marquee text should have some padding before and after the text, to avoid rendering errors, and
	// to allow the text to "flow in" when it is reset after reaching the end.
	private Label lblMarquee = new Label();
		//("                                                                                                 [lblMarquee] Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec metus quam, ullamcorper eu suscipit quis, rutrum sit amet massa.                                                                                                 ");
		//" Nunc dapibus accumsan metus, commodo placerat urna blandit non. Nullam turpis justo, dictum quis dignissim non, vehicula vel lorem. Fusce congue purus odio, lobortis pulvinar neque. Integer dui odio, venenatis sed tincidunt in, fringilla id urna. Maecenas faucibus massa eu orci tincidunt ac vulputate nibh aliquam. Aliquam ullamcorper erat nunc. Vestibulum hendrerit adipiscing neque quis interdum.");
	
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		
		//lblRWinner.Style.FontDescription.Family = "Bitstream Vera Sans Mono";
		
		vpMarquee.BorderWidth = 0;
		vpMarquee.ShadowType = ShadowType.None;
		//vpMarquee.HeightRequest = 40;
		
		vpMarquee.Add (lblMarquee);
		this.vbox1.PackEnd (vpMarquee, false, false, 0);
		
		// update marquee every 33ms (~30fps)
		GLib.Timeout.Add (33, new GLib.TimeoutHandler (MarqueeUpdate));
		
		nbMain.ShowBorder = nbMain.ShowTabs = false;
		nbMain.CurrentPage = 0;
		// connect to the backend
		presenterd = ClientHelper.Connect ();
		settings = ClientHelper.ConnectSettings (presenterd);
		
		// populate fields
		MarqueeUpdate(settings.GetMarqueeText());


		
		// wire events
		presenterd.ClockChange += PresenterdClockChange;
		presenterd.SlideChange += PresenterdSlideChange;
		presenterd.PickWinner += PresenterdPickWinner;
		settings.MarqueeChanged += SettingsMarqueeChanged;
		presenterd.SponsorChange += PresenterdSponsorChange;
		settings.SettingsChanged += SettingsChanged;
		
		SettingsChanged();
		PresenterdSlideChange(presenterd.CurrentSlide);
		
		
		/*
		Console.WriteLine ("Competitions:");
		foreach (SCompetition c in settings.CompetitionsS) {
			Console.WriteLine (new Competition(c));
		}*/
		
		RebuildCompetitionTable();
		
		StartRandomiserText("");
	}

	void PresenterdSponsorChange (string banner_id)
	{
		// load a banner from the current directory
		Console.WriteLine("changing image to {0}.png", banner_id);
		imgSponsor.Pixbuf = new Gdk.Pixbuf(banner_id + ".png");
		imgSponsor.ShowAll();
	}

	void MarqueeUpdate(string val) {
		string padding = new string(' ', 100);
		lblMarquee.Text = padding + val + padding;	
	}
	
	void SettingsMarqueeChanged (string newMarquee)
	{
		MarqueeUpdate(newMarquee);
	}
	
	void SettingsChanged() {
		Application.Invoke(delegate {
			lblEventName.Text = settings.GetEventName();
			tvNotepad.Buffer.Text = settings.GetNotepadText();
		
		
			ShowAll();
		});
	}

	void PresenterdPickWinner (string winner)
	{
		if (presenterd.CurrentSlide == Slide.Randomiser) {
			StartRandomiserText(winner);
		}
	}

	void PresenterdSlideChange (Slide newSlide)
	{
		Application.Invoke(delegate {
			Console.WriteLine("Changing slide to {0}", newSlide);
			switch (newSlide) {
			case Slide.UpcomingCompetitions:
				RebuildCompetitionTable();
				nbMain.CurrentPage = 0;
				break;
				
			case Slide.Servers:
				nbMain.CurrentPage = 1;
				break;
				
			case Slide.Sponsor:
				nbMain.CurrentPage = 2;
				break;
				
			case Slide.Randomiser:
				nbMain.CurrentPage = 3;
				break;
				
			case Slide.Notepad:
				nbMain.CurrentPage = 4;
				break;
				
			case Slide.IronChef:
				nbMain.CurrentPage = 5;
				break;
				
			}
			
			if (newSlide != Slide.Randomiser) {
				// clean up old	data
				lblRTop.Text = "Welcome to...";
				lblRWinner.Text = "THE RANDOMISER!";
				RWinner = "THE RANDOMISER!";
				pbRandomiser.Fraction = 0.0;
				pbRandomiser.Text = "";
				
			}
		});
		
	}
	
	bool MarqueeUpdate ()
	{
		Application.Invoke (delegate {
			// calculate the total amount of space the marquee requires.
			double n = vpMarquee.Hadjustment.Upper - this.WidthRequest;
			if (n < 1)
				// safety incase we don't have an actual width calculated width
				n = 1;
			
			//Console.WriteLine("{0} > {1} ?", vpMarquee.Hadjustment.Value, n);
			if (vpMarquee.Hadjustment.Value >= n) {
				// we've reached the end.  reset the marquee to the 0-position.
				vpMarquee.Hadjustment.Value = 0;
			} else {
				// scroll the marquee to the left by 3px
				vpMarquee.Hadjustment.Value += 3;
			}
			
			// tell gtk# we want it to take into account the updated value.
			vpMarquee.Hadjustment.ChangeValue ();
			vpMarquee.Hadjustment.Change ();
			
			// redraw the control and it's children (the label).
			vpMarquee.ShowAll ();
			
			if (RandomiserPicking && vpMarquee.Hadjustment.Value % 2 == 0)
				ShuffleRandomiserText();
		});
		
		// tell Timeout we want to be called again.
		return true;
	}

	void PresenterdClockChange (double time)
	{
		DateTime t = ClientHelper.UnixToDateTime (time);
		Application.Invoke (delegate {
			// update clock
			lblClock.Text = t.ToString (settings.GetClockTimeFormat());
			
			// update event progress
			pbEventProgress.Style.FontDesc.Size = 20000;
			pbEventProgress.Style.FontDesc.Weight = Pango.Weight.Bold;

			
			DateTime start = ClientHelper.UnixToDateTime(settings.GetStartTime());
			DateTime finish = ClientHelper.UnixToDateTime(settings.GetFinishTime());
			TimeSpan remaining = finish - t;
			TimeSpan totalTime = finish - start;
			TimeSpan elapsed = t - start;
			if (remaining.TotalSeconds > 0) {
				if (elapsed.TotalSeconds > 0) {
					pbEventProgress.Fraction = elapsed.TotalSeconds / totalTime.TotalSeconds;
					pbEventProgress.Text = string.Format("{0} hour{1} and {2} minute{3} remaining in this event", Math.Floor(remaining.TotalHours), Math.Floor(remaining.TotalHours) == 1 ? "": "s", remaining.Minutes, remaining.Minutes == 1 ? "" : "s"/*, remaining.Seconds, remaining.Seconds == 1 ? "": "s"*/);
				} else {
					pbEventProgress.Fraction = 0.0;
					pbEventProgress.Text = "Not started yet";
				}
			} else {
				pbEventProgress.Fraction = 1.0;
				pbEventProgress.Text = "Finished!";
			}
			
			pbEventProgress.ShowAll();
			lblClock.ShowAll();
					
		});
	}
	
	
	protected void RebuildCompetitionTable() {
		Console.WriteLine("Rebuilding competition table...");
		Competition[] competitions = Competition.SCompetitionsToCompetitions(settings.CompetitionsS);
		List<Competition> current = new List<Competition>();
		List<Competition> upcoming = new List<Competition>();
		
		
		foreach (Competition c in competitions) {
			if (c.Upcoming) {
				upcoming.Add(c);
			} else if (c.IsNow) {
				current.Add(c);
			}
		}
		
		current.Sort();
		upcoming.Sort();
		
		Console.WriteLine("RebuildCompetitionTable: {0} current, {1} upcoming", current.Count, upcoming.Count);
		
		// Clear out old stuff
		foreach (Widget w in CompetitionsTable.Children) {
			CompetitionsTable.Remove(w);
		}
		
		List<Competition> finalList = new List<Competition>();
		finalList.AddRange(current);
		finalList.AddRange(upcoming);
		
		// only show 8 items
		if (finalList.Count > 8) {
			finalList.RemoveRange(8, finalList.Count - 8);
		}
		
		
		// timestyle
		Style ts = Style.Copy();
		ts.FontDesc.Size = 30000;
		ts.FontDesc.Weight = Pango.Weight.Bold;
		
		// competition name style
		Style ns = Style.Copy();
		ns.FontDesc.Size = 30000;
		
		// progress bar text style
		Style pbs = Style.Copy();
		pbs.FontDesc.Size = 30000;
		
		
		uint row = 0;
		
		lock (CompetitionsTable) {
			if (finalList.Count > 0) {
				foreach (Competition c in finalList) {
					Label l = new Label(c.Start.ToString(settings.GetCompetitionTimeFormat()));
					l.Style = ts;
					CompetitionsTable.Attach(l, 0, 1, row, row+1, AttachOptions.Shrink, AttachOptions.Shrink, 10, 10);
					
					l = new Label();
					l.Markup = string.Format("{0} <span face=\"Console Logos\">{1}</span>", ClientHelper.EscapeXML(c.Name), ClientHelper.EscapeXML(c.Platforms));
					l.Style = ns;	
		
					CompetitionsTable.Attach(l, 1, 2, row, row+1);
					
					ProgressBar b = new ProgressBar();
					b.Style = pbs;
					if (c.IsNow) {
						//b.Fraction = 1.0;
						b.Text = "ends in " + ClientHelper.TimeFormat(c.FinishesIn);

						// show time remaining
						b.Fraction = c.FractionRemaining;
					} else {
						b.Fraction = 0.0;
						b.Text = "starts in " + ClientHelper.TimeFormat(c.StartsIn);
					}
					CompetitionsTable.Attach(b, 2, 3, row, row+1);
					row++;
				}
			} else {
				// no events
				Label l = new Label("There are no scheduled upcoming events.");
				l.Style = ts;
				CompetitionsTable.Attach(l, 0, 1, 0, 1);
			}
			
			//SAGAPresenter.CGServer.Main.SendCompetitionsScreen(finalList);

		}
			
		/*
		foreach (Widget w in CompetitionsTable.Children) {
			w.Style.FontDesc.Size = 20000;
		}*/
		CompetitionsTable.ShowAll();
	}

	private string RWinner = string.Empty;
	private bool RandomiserPicking = false;
	private const string RandomiserCharacters = //"0123456789PYFGCRLAOEUIDHTNSQJKXBMWVZpyfgcrlaoeuidhtnsqjkxbmwvz _-";
		"0зiOЩΞyﾑКоСτΘ8ГﾄфnｵTｬﾒwΠｱｴАCeｹγｯΤиkюθΓψfZﾅΙuκｨцσдπHЮﾜzFﾎsЧ_MﾀУﾖΝбεοﾉВЕιﾍХWυｾｪШДЬﾁqΡ4аﾕлζηχ3GﾋbﾐﾈМA5ωвﾇяｧﾔЖ7ﾌﾚ1ﾝｭQЛｼйхﾊρλｺПｶΥЫчβｷъуΕXФпжнU6PmJaSБlеg9ИIΨo2ｮΚdс-ОРΑмﾆ ｻщкﾏﾘｦhшjΟYφrDｩLЭEtΗμΩΧΔгpЯBэΜRςｫытδΖVТΒｽνxЗﾛЪﾙﾃｳьｿﾓрαξｲΦKЙcΛΣНﾗЦNﾂｸv";
	
	private static Random rng = new Random();
	
	private string RandomString(int length) {
		string o = string.Empty;
		for (int x = 0; x<length; x++) {
			o += RandomiserCharacters[rng.Next(RandomiserCharacters.Length)];	
		}
		return o;
	}
	
	private void StartRandomiserText(string winner) {
		// start by setting up the random text to be displayed
		this.RWinner = winner;
		//char[] start = RandomString(winner.Length).ToCharArray();
		StringBuilder start = new StringBuilder(RandomString(winner.Length));
		
		// now get the "non-randomisable" characters
		for (int x=0; x<winner.Length; x++) {
			if (!RandomiserCharacters.Contains(winner[x].ToString())) {
				start[x] = winner[x];
			}
		}
		
		// now set the label to that
		lblRTop.Text = "Randomising winner...";
		lblRWinner.Text = start.ToString();
		RandomiserPicking = true;
		pbRandomiser.Fraction = 0.0;
		
		lblRTop.ShowAll();
		lblRWinner.ShowAll();
	}
	
	private bool ShuffleRandomiserText() {
		// rotate all the characters so it's closer to finding the winner.
		// returns true if the winner is found.
		int CharactersFound = 0;
		StringBuilder newPick = new StringBuilder(lblRWinner.Text);
		for (int x=0; x<RWinner.Length; x++) {
			if (newPick[x] == RWinner[x]) {
				CharactersFound++;
			} else {
				// this character needs rotating
				// find it's position in the array of valid chars
				int position = RandomiserCharacters.IndexOf(newPick[x]);
				
				// do we move up or down?
				if (x % 2 == 0) {
					// down
					position--;
					if (position < 0)
						position = RandomiserCharacters.Length - 1;
				} else {
					// up
					position++;
					if (position >= RandomiserCharacters.Length)
						position = 0;
				}
				
				// replace the character
				newPick[x] = RandomiserCharacters[position];
			}
			
		}
		
		lblRWinner.Markup = string.Format("<span size=\"60000\">{0}</span>", newPick.ToString());
		pbRandomiser.Fraction = (double)CharactersFound / (double)RWinner.Length;
		pbRandomiser.Text = string.Format("{0} of {1} characters found", CharactersFound, RWinner.Length);
		
		if (CharactersFound == RWinner.Length)
			lblRTop.Text = "And the winner is...";
		return lblRWinner.Text == RWinner;
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}

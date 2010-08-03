#!/usr/bin/env python
# pygame client to sagapresenter2

import dbus, pygame, dbus.service, dbus.mainloop.glib, gobject
from pygame.locals import *
SCREEN_RESOLUTION = (1024, 768)
SP2 = 'au.id.micolous.sp2'
PRESENTERD = SP2 + ".presenterd"
DBUS = USEREVENT


	

# pygame-dbus integration notes:
# <http://rene.f0o.com/~rene/stuff/dbus/pygame_dbus.py>
dbus.mainloop.glib.DBusGMainLoop(set_as_default=True)
bus = dbus.SessionBus()

proxy = bus.get_object(SP2, '/presenterd')
iface = dbus.Interface(proxy, PRESENTERD)
loop = gobject.MainLoop()
gobject.threads_init()
context = loop.get_context()

def on_clockchange(time):
	pygame.event.post(pygame.event.Event(DBUS, name="ClockChange", time=time))
proxy.connect_to_signal("ClockChange", on_clockchange)

def on_slidechange(new_slide):
	pygame.event.post(pygame.event.Event(DBUS, name="SlideChange", new_slide=new_slide))
proxy.connect_to_signal("SlideChange", on_slidechange)

pygame.init()
pygame.mixer.quit()

screen = pygame.display.set_mode(SCREEN_RESOLUTION, 0)
pygame.display.set_caption("SAGAPresenter2 pygclient")

print "properties: %s" % iface.GetActiveSettings()

going = True

while going:
	for e in pygame.event.get():
		if e.type in [QUIT, KEYDOWN]:
			going = False
		elif e.type is DBUS:
			print "Got dbus event, type %s" % e.name
			if e.name == "ClockChange":
				print "new time is %s" % e.time
			elif e.name == "SlideChange":
				print "new slide is %s" % e.new_slide
				
	were_events_dispatched = context.iteration(False)
	pygame.display.flip()

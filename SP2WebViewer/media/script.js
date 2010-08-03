var clock;
var days = Array("Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat");

function leading_zero(val) {
	if (val < 10)
		return "0" + val;
	return val;
}

function update_clock() {
	now = new Date();
	clock.innerHTML = days[now.getDay()] + ", " +
		leading_zero(now.getHours()) + ":" +
		leading_zero(now.getMinutes()) + ":" + 
		leading_zero(now.getSeconds());

	setTimeout("update_clock()", 1000);
}

window.addEventListener('load', function() {
	clock = document.getElementById('clock');
	update_clock();
}, false);
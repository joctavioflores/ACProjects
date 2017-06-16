// Colorise all text/password input boxes on a page
// License: http://www.gnu.org/licenses/lgpl.txt
// Homepage: http://blog.leenix.co.uk/2009/07/jquery-onfocusonblur-text-box-color.html
// Version 1.02


jQuery(document).ready(function() {
	jQuery("input:[type=text], input:[type=password]").focus(function () {
		jQuery(this).addClass("highLightInput");	
	});
	jQuery("input:[type=text], input:[type=password]").blur(function () {
		jQuery(this).removeClass("highLightInput");
	});
});



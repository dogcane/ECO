///////////////////////////////////////////////
//Localizer V0.1
///////////////////////////////////////////////
(function ($) {
    $.localizer = function (resourcePattern, currentLanguage) {

        var realPattern = resourcePattern.replace('[language]', currentLanguage);
        var fallbackPattern = resourcePattern.replace('[language].','');
        $.getScript(realPattern)
            .fail(function (jqxhr, settings, exception) {
                $.getScript(fallbackPattern);
            });

    };
})(jQuery);
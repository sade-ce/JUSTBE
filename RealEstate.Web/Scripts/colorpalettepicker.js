(function ($) {
    "use strict";

    var paletteObj = {
        aqua: '#800000',
        azure: '#808000',
        beige: '#008000',
        black: '#800080',
        blue: '#008080',
        brown: '#FF00FF',
     
    }

    var methods = {
        init: function (params) {
            const defaults = $.fn.colorPalettePicker.defaults;
            if (params.bootstrap == 3) {
                $(this).addClass('dropdown');
                defaults.buttonClass = 'btn btn-default dropdown-toggle';
                defaults.button = '<button id="colorpaletterbuttonid" name="colorpalettebutton" class="{buttonClass}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"><span name="{buttonPreviewName}" style="display:none">■ </span>{buttonText} <span class="caret"></span></button>';
                defaults.dropdown = '<ul class="dropdown-menu" aria-labelledby="colorpaletterbuttonid"><h5 class="dropdown-header text-center">{dropdownTitle}</h5>';
                defaults.menu = '<ul class="list-inline" style="padding-left:10px;padding-right:10px">';
                defaults.item = '<li><div name="picker_{name}" style="background-color:{color};width:32px;height:32px;border-radius:5px;border: 1px solid #666;margin: 0px;cursor:pointer" data-toggle="tooltip" title="{name}" data-color="{color}"></div></li>';
            }
            const options = $.extend({}, defaults, params);

            // button configuration
            const btn = $(options.button
                .replace('{buttonText}', options.buttonText)
                .replace('{buttonPreviewName}', options.buttonPreviewName)
                .replace('{buttonClass}', options.buttonClass));
            $(this).html(btn);
            // dropdown configuration
            const dropdown = $(options.dropdown.replace('{dropdownTitle}', options.dropdownTitle));
            // check if colors passed throught data-colors
            const dataColors = $(this).attr('data-colors');
            if (dataColors != undefined) {
                options.palette = dataColors.split(',');
            }
            // check if lines passed throught data-lines
            const dataLines = $(this).attr('data-lines');
            if (dataLines != undefined)
                options.lines = dataLines;
            // calculating items per line
            const paletteLength = options.palette.length;
            const itemsPerLine = Math.round(paletteLength / options.lines);
            let paletteIndex = 0;
            for (let i = 0; i < options.lines; i++) {
                const menu = $(options.menu);

                for (let j = 0; j < itemsPerLine; j++) {
                    const paletteObjItem = paletteObj[options.palette[paletteIndex]];
                    if (paletteObjItem != undefined) {
                        menu.append(options.item.replace(/{name}/gi, options.palette[paletteIndex]).replace(/{color}/gi, paletteObjItem));
                    }
                    paletteIndex++;
                }
                dropdown.append(menu);
            }
            $(this).append(dropdown);
            // item click bindings
            $(this).find('div[name^=picker_]').on('click',
                function () {
                    const selectedColor = $(this).attr('data-color');
                    const colorSquare = $('span[name=' + options.buttonPreviewName + ']');
                    colorSquare.css('color', selectedColor);
                    if (!colorSquare.is(':visible'))
                        colorSquare.show();
                    if (typeof options.onSelected === 'function') {
                        options.onSelected(selectedColor);
                    }
                });
        }
    }

    $.fn.colorPalettePicker = function (options) {
        if (methods[options]) {
            return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof options === 'object' || !options) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Option ' + options + ' not found in colorPalettePicker');
        }
    };

    $.fn.colorPalettePicker.defaults = {
        button: '<button name="colorpalettebutton" class="{buttonClass}" data-toggle="dropdown"><span name="{buttonPreviewName}" style="display:none">■ </span>{buttonText}</button>',
        buttonClass: 'btn btn-secondary dropdown-toggle',
        buttonPreviewName: 'colorpaletteselected',
        buttonText: 'Choose color',
        dropdown: '<div class="dropdown-menu"><h5 class="dropdown-header text-center">{dropdownTitle}</h5>',
        dropdownTitle: 'Available colors',
        menu: '<ul class="list-inline" style="padding-left:10px;padding-right:10px">',
        item: '<li class="list-inline-item"><div name="picker_{name}" style="background-color:{color};width:32px;height:32px;border-radius:5px;border: 1px solid #666;margin: 0px;cursor:pointer" data-toggle="tooltip" title="{name}" data-color="{color}"></div></li>',
        palette: ['aqua', 'azure', 'beige', 'brown', 'cyan', 'darkcyan', 'darkgrey', 'darkkhaki', 'darkorange', 'darkorchid', 'darksalmon', 'fuchsia', 'gold', 'green', 'khaki', 'lightblue', 'lightcyan', 'lightgreen', 'lightgrey', 'lightpink', 'lightyellow', 'lime', 'magenta', 'olive', 'orange', 'pink', 'silver', 'yellow'],
        lines: 1,
        bootstrap: 4,
        onSelected: null
    };
})(jQuery);
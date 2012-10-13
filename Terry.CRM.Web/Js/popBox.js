/*
* jQuery popBox
* Copyright (c) 2010 Simon Hibbard
* 
* Permission is hereby granted, free of charge, to any person
* obtaining a copy of this software and associated documentation
* files (the "Software"), to deal in the Software without
* restriction, including without limitation the rights to use,
* copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following
* conditions:

* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
* OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
* HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
* WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE. 
*/

/*
* Version: V1.1.0
* Release: 06-08-2010
* Based on jQuery 1.4.2
*/

(function ($) {
    $.fn.popBox = function (options) {

        var defaults = {
            height: 100,
            width: 300,
            newlineString: "\n"
        };
        var options = $.extend(defaults, options);

        return this.each(function () {

            obj = $(this);
            obj.after('<div class="popBox-holder"></div><div class="popBox-container"><textarea class="popBox-input" /><div class="done-button"><input type="button" value="Done" class="button blue small"/></div></div>');

            obj.focus(function () {
                $(this).next(".popBox-holder").show();

                $(this).next().next(".popBox-container").children('.popBox-input').css({ height: options.height, width: options.width });
                $(this).next().next(".popBox-container").show();

                var scrolled = $(document).scrollTop();
                var top = ($(window).height() - $(this).next().next(".popBox-container").outerHeight()) / 2 + scrolled;
                var left = ($(window).width() - $(this).next().next(".popBox-container").outerWidth()) / 2;
                $(".popBox-holder").css({ top: scrolled });
                $(this).next().next(".popBox-container").css({ position: 'absolute', margin: 0, top: (top > 0 ? top : 0) + 'px', left: (left > 0 ? left : 0) + 'px' });

                $(this).next().next(".popBox-container").children('.popBox-input').val($(this).val().replace(RegExp(options.newlineString,"g"), "\n"));
                $(this).next().next(".popBox-container").children('.popBox-input').focus();

                $(this).next().next(".popBox-container").children().blur(function () {
                    $(this).parent().hide();
                    $(this).parent().prev().hide();

                    $(this).parent().prev().prev().val($(this).val().replace(/\n/g, options.newlineString));
                });

            });

        });

    };

})(jQuery);

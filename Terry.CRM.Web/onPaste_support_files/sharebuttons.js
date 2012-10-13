
// -------------------------------------------------------------------
// Madgex Limited
// Copyright (c) 2011 Madgex Limited. All Rights Reserved.
// Common, version 0.1 
// Glenn Jones
// Finds and replaces all intents with buttons
// -------------------------------------------------------------------


// If we have javascript - hide the text links
var head = document.getElementsByTagName('head');
if (head) {
    var text = '.intent-buttons a{text-indent:-9999px;display:block}'
    var style = document.createElement('style');
    style.type = 'text/css';
    style.media = 'screen';
    head[0].appendChild(style);
    if (style.styleSheet) {
        style.styleSheet.cssText = text;
    } else {
        style.appendChild(document.createTextNode(text));
    }

}



// Only continue if jQuery has been loaded
if (typeof jQuery != 'undefined') {


    var MadgexIntents = (function (m) {
        m.version = 0.1;
        m.intents = [];

        //PNG fix
        DD_belatedPNG.fix('.intent-count, .intent-button')

        // Configation - twitter, linkedin, facebook and google+
        var twitter = {}
        twitter.verb = 'share'
        twitter.type = 'madgex'
        twitter.vendor = 'Twitter'
        twitter.url = 'https://twitter.com/intent/tweet?original_referer={urlcanonical}&source=tweetbutton&text={description}&url={urlshort}';
        // Optional
        twitter.counturl = 'http://urls.api.twitter.com/1/urls/count.json?url={urlcanonical}&callback=?';
        twitter.getCount = function (data) {
            if (data.count) {
                return data.count;
            } else {
                return 0;
            }
        };
        twitter.window = { type: 'window', width: 560, height: 350 };
        m.intents.push(twitter);


        var linkedin = {}
        linkedin.verb = 'share'
        linkedin.type = 'madgex'
        linkedin.vendor = 'Linkedin'
        linkedin.url = 'http://www.linkedin.com/shareArticle?mini=true&url={urlshort}&title={title}&ro=false&summary={description}';
        linkedin.counturl = 'json-proxy/?url=' + encodeURIComponent('http://www.linkedin.com/cws/share-count?url=') + '{urlcanonical}&oldmethod=IN.Tags.Share.handleCount&callback=?'
        linkedin.getCount = function (data) {
            if (data.count) {
                return data.count;
            } else {
                return 0;
            }
        };
        linkedin.window = { type: 'window', width: 600, height: 420 };
        m.intents.push(linkedin);


        var facebook = {}
        facebook.verb = 'share'
        facebook.type = 'madgex'
        facebook.vendor = 'Facebook'
        facebook.url = 'http://www.facebook.com/sharer/sharer.php?u={urlshort}&t={title}';
        // Optional
        facebook.counturl = 'https://api.facebook.com/method/fql.query?query=select%20%20like_count,%20total_count,%20share_count,%20click_count%20from%20link_stat%20where%20url="{urlshort}"&format=json&callback=?';
        facebook.getCount = function (data) {
            if (data.length) {
                return data[0].total_count;
            } else {
                return 0;
            }
        };
        facebook.window = { type: 'window', width: 560, height: 350 };
        m.intents.push(facebook);


        var google = {}
        google.verb = 'share'
        google.type = 'google'
        google.vendor = 'Google+'
        google.scriptsrc = ['https://apis.google.com/js/plusone.js']
        google.html = '<g:plusone size="{format}" count="false"></g:plusone>';
        m.intents.push(google);


        m.setup = function () {
            // Find links in the document that have the data-verb attribute
            $('a[data-verb]').each(function (index) {
                var x = m.intents.length;
                while (x--) {
                    if (m.intents[x].verb == $(this).attr('data-verb') && m.intents[x].vendor == $(this).attr('data-vendor'))
                        buildButton($(this), m.intents[x]);
                }
            });
        };


        var buildButton = function (elt, intent) {
            // Create new values so they can be modified 
            var verb = $(elt).attr('data-verb');
            var vendor = $(elt).attr('data-vendor');
            var format = $(elt).attr('data-format');
            var urlShort = $(elt).attr('data-url-short');
            var urlCanonical = $(elt).attr('data-url-canonical');
            var title = $(elt).attr('data-title');
            var description = $(elt).attr('data-description');
            var text = $(elt).html();
            var linkTitle = $(elt).attr('title');

            var intentUrl = intent.url;
            intentUrl = replaceWithValue(intentUrl, 'urlshort', encodeURIComponent(urlShort));
            intentUrl = replaceWithValue(intentUrl, 'urlcanonical', encodeURIComponent(urlCanonical));
            intentUrl = replaceWithValue(intentUrl, 'title', encodeURIComponent(title));
            intentUrl = replaceWithValue(intentUrl, 'description', encodeURIComponent(description));
            var classStr = 'intent-' + justSimpleChars(vendor) + '-' + format + '-' + verb;

            // Up to here the rest need turning into jquery
            if (format.indexOf('-count') > 1) {
                $(elt).replaceWith('<div class="' + classStr + '"><span class="intent-count"> </span><span class="intent-button"> </span></div>');
            } else {
                $(elt).replaceWith('<div class="' + classStr + '"></div>');
            }
            var div = $('.' + classStr);
            var countSpan = $('.' + classStr + ' .intent-count');
            var buttonSpan = $('.' + classStr + ' .intent-button');

            if (linkTitle) {
                $(div).attr('title', linkTitle);
            }

            if (intent.html) {
                var html = replaceWithValue(intent.html, 'format', format);
                $(div).html(html);
            }

            if (intent.scriptsrc) {
                var j = 0;
                while (j < intent.scriptsrc.length) {
                    $.getScript(intent.scriptsrc[j]);
                    j++;
                }
            }

            if (intent.counturl) {
                var countUrl = replaceWithValue(intent.counturl, 'urlshort', encodeURIComponent(urlShort));
                countUrl = replaceWithValue(countUrl, 'urlcanonical', encodeURIComponent(urlCanonical));

                $.getJSON(countUrl, function (data) {
                    //alert(intent.vendor + JSON.stringify(data));
                    if (intent.getCount) {
                        if (format.indexOf('-count') > 1) {
                            $(countSpan).html(intent.getCount(data));
                        }
                        if (intent.getCount(data) > 0) {
                            if (intent.getCount(data) == 1) {
                                $('.' + classStr).attr('title', linkTitle + ' (1 person has shared this)');
                            } else {
                                $('.' + classStr).attr('title', linkTitle + ' (' + intent.getCount(data) + ' people have shared this)');
                            }
                        }
                    }
                });

            }

            var targetElt = div;
            if (format.indexOf('-count') > 1) {
                targetElt = buttonSpan;
                $(countSpan).html('0');
            }


            $(targetElt).click(function () {
                // Googles Analytics Social Tracking
                if (window['_gaq']) {
                    _gaq.push(['_trackSocial', vendor, verb, document.location.href])
                }
                if (intent.window) {
                    var winOptions = 'menubar=yes, location=yes, resizable=yes, scrollbars=yes, status=yes';
                    if (intent.window.width) {
                        winOptions += ', width=' + intent.window.width;
                    }
                    if (intent.window.height) {
                        winOptions += ', height =' + intent.window.height;
                    }
                    var win = window.open(intentUrl, '_blank', winOptions);
                } else {
                    docuemnt.location.href = intentUrl;
                }
            });

            $(targetElt).mouseover(function () {
                $(this).addClass('add: ' + classStr + '-over');
            });

            $(targetElt).mouseout(function () {
                $(this).removeClass('add: ' + classStr + '-over');
            });

        }

        var replaceWithValue = function (url, name, value) {
            if (url && value != null) {
                if (url.indexOf('{' + name + '}') > -1) {
                    var reg = new RegExp('{' + name + '}','gi')
                    return url.replace(reg, value)
                } else {
                    return url;
                }
            } else {
                return url;
            }
        }

        var justSimpleChars = function (str) {
            return str.toLowerCase().replace(/[^a-z,0-9,-]/g, "");
        };


        return m;

    } (MadgexIntents || {}));



    $(document).ready(function () {
        MadgexIntents.setup();
    });


}
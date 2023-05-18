/// <reference path="http://code.jquery.com/qunit/qunit-1.18.0.js" />


$.fn.responsiveTable = function (options) {

    return this.each(function (i, el) {

        var table = el;

        var startNavigationButton = $(table).find(".responsive_table_start_navigation_button");
        var leftNavigationButton = $(table).find(".responsive_table_left_navigation_button");
        var rightNavigationButton = $(table).find(".responsive_table_right_navigation_button");
        var endNavigationButton = $(table).find(".responsive_table_end_navigation_button");
        var searchNavigationButton = $(table).find(".responsive_table_search_navigation_button");
        var filterNavigationButton = $(table).find(".responsive_table_filter_navigation_button");
        var scrollTracker = $(table).find(".responsive_table_scroll_tracker");
        var scrollIndex = 0;
        var maxScrollIndex = function () {
            return $(table).children("tbody").children("tr").length - 1;
        }

        table.init = function () {
            /* —
                Function: performs initial operations necessary to configure the responsive table
                
                Input: none
            — */

            $(table).addClass("responsive_table");

            // click events
            startNavigationButton.on("click", function () {

                table.scrollToIndex(0, true);

            });

            leftNavigationButton.on("click", function () {

                table.scrollToIndex(--scrollIndex, true);

            });

            rightNavigationButton.on("click", function () {

                table.scrollToIndex(++scrollIndex, true);

            });

            endNavigationButton.on("click", function () {

                table.scrollToIndex(maxScrollIndex(), true);

            });

            searchNavigationButton.on("click", function () {



            });

            filterNavigationButton.on("click", function () {



            });

            // window resize response
            $(window).on("resize", function () {

                table.determineScrollability();

            });

            // force header and footer
            table.forceHeaderAndFooter();

            // determine table scrollability
            table.determineScrollability();

            // configure navigation buttons
            table.configureNavigationButtons();

            // update scroll tracker
            table.updateScrollTracker();

            // fix table offset (pending...)
            table.fixOffset();

        };

        table.forceHeaderAndFooter = function () {
            /* —
                Function: Forces the thead and tfoot elements on the table
                
                Input: none
            — */

            // check for header
            if (!$(table).children("thead").length) {
                $(table).children("tbody").each(function () {
                    var responsiveHeader = jQuery("<thead>");

                    // picks out only table rows with table headers inside of them
                    $(this).children("tr:first-of-type").children("th").parent().appendTo(responsiveHeader);

                    responsiveHeader.find("th").addClass("clickable");
                    responsiveHeader.insertBefore($(this));
                });
            }

            /* check for footer
            if (!$(table).children("tfoot").length) {
                var responsiveFooter = jQuery("<tfoot>").append("<tr>");

                for (var i = 0; i < $(table).find("th").length; ++i) {

                    var footerItem = jQuery("<td><i></i></td>");

                    switch (i) {
                        case 0:
                            footerItem.addClass("clickable animated responsive_table_navigation_button responsive_table_start_navigation_button");
                            footerItem.children("i").addClass("fa fa-angle-double-left");
                            break;

                        case 1:
                            footerItem.addClass("clickable animated responsive_table_navigation_button responsive_table_left_navigation_button");
                            footerItem.children("i").addClass("fa fa-chevron-left");
                            break;

                        case $(table).find("th").length - 2:
                            footerItem.addClass("clickable animated responsive_table_navigation_button responsive_table_right_navigation_button");
                            footerItem.children("i").addClass("fa fa-chevron-right");
                            break;

                        case $(table).find("th").length - 1:
                            footerItem.addClass("clickable animated responsive_table_navigation_button responsive_table_end_navigation_button");
                            footerItem.children("i").addClass("fa fa-angle-double-right");
                            break;

                        default:
                            break;
                    }

                    responsiveFooter.children("tr").append(footerItem);
                }

                responsiveFooter.appendTo($(table));
            } */
        }

        table.determineScrollability = function () {
            /* —
                Function: Determines whether or not the table should be scrollable
                and adds or removes the 'scrollable' class accordingly
                
                Input: none
            — */

            if ($(this).children("thead").find("th").length * 120 > $(window).width()) {

                if (!$(this).hasClass("scrollable")) {
                    $(this).addClass("scrollable");

                }

            } else {

                if ($(this).hasClass("scrollable"))
                    $(this).removeClass("scrollable");

            }
        };

        table.fixOffset = function () {
            /* —
                Function: Adjusts the table body and rows to account for the max scroll offset
                
                Input: none
            — */

            var offset = ($(table).children("tbody").outerWidth() - $(table).width()) - ($(table).children("thead").width() + $(table).children("tfoot").width());

            //$(table).children("tbody").css("left", offset + "px");

        };

        table.scrollToIndex = function (index, animated) {
            /* —
                Function: Scrolls the table to the given index
                
                Input:
                ... index - the index the table should scroll to
                ... animated - a boolean value indicating whether
                the table scrolling should be animated
            — */

            // catch bad indices
            if (index < 0) {
                index = 0;
            } else if (index > maxScrollIndex()) {
                index = maxScrollIndex();
            }

            // calculates duration based upon distance
            var distance = Math.max(Math.abs(parseInt($(table).children("tbody").css("margin-left").replace("px", ""))), $(table).children("tbody").children("tr").eq(index).position().left) - Math.min(Math.abs(parseInt($(table).children("tbody").css("margin-left").replace("px", ""))), $(table).children("tbody").children("tr").eq(index).position().left);
            var duration =
                (animated)
                ? 400

                : 0;

            // determine destination
            var destination = -$(table).children("tbody").children("tr").eq(index).position().left

            // scroll to destination with duration
            $(table).children("tbody").stop().animate({
                "margin-left": destination + "px"
            }, {
                duration: duration,
                easing: 'easeOutQuint'/*,
                step: function () {
                    if (parseInt($(table).children("tbody").css("margin-left").replace("px", "")) < -($(table).children("tbody").outerWidth() - $(table).width())) {
                        $(table).children("tbody").stop().animate({
                            "margin-left": -($(table).children("tbody").outerWidth() - $(table).width()) - ($(table).children("thead").width() + $(table).children("tfoot").width()) + "px"
                        }, {
                            duration: 200
                        });
                    }
                }*/

            });

            // maintain scrollIndex
            scrollIndex = index;

            // determine appropriate states for navigation buttons
            table.configureNavigationButtons();

            // update scroll tracker
            table.updateScrollTracker();

            // execute callback
            if (typeof callback == "function") callback();
        };

        table.updateScrollTracker = function () {
            /* —
                Function: Updates the responsive table's scroll tracker
                
                Input: none
            — */

            // update scroll tracker
            scrollTracker.children("span").stop().animate({
                "border-left-width": ((scrollIndex / maxScrollIndex()) * scrollTracker.children("span").outerWidth()) + "px"
            }, {
                duration: 100
            });

        };

        table.configureNavigationButtons = function () {
            /* —
                Function: enables and disables navigation buttons based
                upon table scroll state
                
                Input: none
            — */

            // start and left navigation button
            if (scrollIndex > 0)
                $(this).find(".responsive_table_start_navigation_button, .responsive_table_left_navigation_button").removeClass("disabled");
            else
                $(this).find(".responsive_table_start_navigation_button, .responsive_table_left_navigation_button").addClass("disabled");

            // right and end navigation button
            if (scrollIndex < maxScrollIndex())
                $(this).find(".responsive_table_right_navigation_button, .responsive_table_end_navigation_button").removeClass("disabled");
            else
                $(this).find(".responsive_table_right_navigation_button, .responsive_table_end_navigation_button").addClass("disabled");

        }

        table.init();

    });

}

// convert all elements in responsive table class
$(document).ready(function () {
    $("table.responsive_table").responsiveTable();
});
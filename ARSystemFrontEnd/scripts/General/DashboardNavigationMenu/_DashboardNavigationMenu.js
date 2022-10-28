var openNavMenu = false;

//event dashboard navigation
$(document).mouseup(function (e) {
    var container = $(".hor-menu #header_menu_bar");
    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        if ($("#_ddmDashboardNavigationMenu").is(":visible"))
            $(".overlay_dashboard_menu, #_ddmDashboardNavigationMenu").toggle('slide', { direction: 'up' }, 500);
    }
});

$("#_dashboardNavigationMenu").mouseenter(function (e) {
    e.preventDefault();
    if ($("#_ddmDashboardNavigationMenu").is(":visible"))
        $("#_ddmDashboardNavigationMenu").hide();
});

$("#_dashboardNavigationMenu").click(function (e) {
    e.preventDefault();
    $(".overlay_dashboard_menu, #_ddmDashboardNavigationMenu").toggle('slide', { direction: 'up' }, 500);
    $("#_divresultMenu").empty();
    $("#_tbxSearchMenu").val("");
    if (openNavMenu == false) {
        DashboardNavigationMenu.Init();
        DashboardNavigationMenu.ApplicationList();
        DashboardNavigationMenu.FavoriteMenuList();
        openNavMenu = true;
    }
});
//end event

var DashboardNavigationMenu = {
    Init: function () {
        $('#_divDashboardNavigationMenu').slimScroll({
            height: 'calc(-150px + 100vh)'
        });

        $('#_tbxSearchMenu').keypress(function (e) {
            if (e.which == 13) {
                if ($("#_tbxSearchMenu").val().trim() != "")
                    DashboardNavigationMenu.Search();
            }
        });

        $(document).on('click', '.check-fav', function () {
            $(this).css("color", "#d5a30d");
            $(this).addClass('uncheck-fav');
            $(this).removeClass('check-fav');

            let data = $(this);
            let menuId = data.attr('data-menu-id');
            let appId = data.attr('data-app-id');
            console.log(menuId + '    ' + appId);
            DashboardNavigationMenu.CreateFavMenu(menuId, appId);
        });

        $(document).on('click', '.uncheck-fav', function () {
            $(this).css("color", "#BABABA");
            $(this).addClass('check-fav');
            $(this).removeClass('uncheck-fav');

            let data = $(this);
            let menuId = data.attr('data-menu-id');
            let appId = data.attr('data-app-id');
            console.log(menuId + '    ' + appId);
            DashboardNavigationMenu.DeleteFavMenu(menuId, appId);
        });

        $(document).on('click', '.legacy-url', function () {
            let data = $(this);
            let config = data.attr('data-config');
            window.open('/Legacy?config=' + config + '&returnUrl=Default.aspx', '_blank');
        });
    },
    Search: function () {
        App.blockUI({ target: "#_divbodySearchMenu", animate: !0 });
        $.ajax({
            url: "/api/dashboardNavigationMenu/getMenuList",
            type: "GET",
            data: {
                keyword: $("#_tbxSearchMenu").val().trim()
            },
        }).done(function (data, textStatus, jqXHR) {
            $("#_divresultMenu").html(data);
            App.unblockUI("#_divbodySearchMenu");
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            App.unblockUI("#_divbodySearchMenu");
        });
    },
    ApplicationList: function () {
        $("#new-system-app").empty();
        $.ajax({
            url: "/api/dashboardNavigationMenu/getApplicationList",
            type: "GET",
        }).done(function (data, textStatus, jqXHR) {
            console.log(data);
            let html = '';
            $.each(data, function (idx, itm) {
                html = '<div class="application-item"><img src="/Content/images/logoDashboardMenu.png" class="application-img"><a href="' + itm.ApplicationURL + '" target="_blank">' + itm.ApplicationName + '</a></div >';
                $("#new-system-app").append(html);
                if (itm.ApplicationName == "TBiGSys")
                    $("#hideURLTBiGSys").val(itm.ApplicationURL);
            });

            $(".applications-carousel").owlCarousel({
                dots: true,
                responsive: {
                    0: {
                        items: 2
                    },
                    768: {
                        items: 3
                    },
                    900: {
                        items: 4
                    }
                }
            });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
        });
    },
    FavoriteMenuList: function () {
        App.blockUI({ target: "#_divbodyFavoriteMenu", animate: !0 });
        $.ajax({
            url: "/api/dashboardNavigationMenu/getFavoriteMenuList",
            type: "GET",
        }).done(function (data, textStatus, jqXHR) {
            $("#_divFavoritetMenu").html(data);
            App.unblockUI("#_divbodyFavoriteMenu");
        }).fail(function (jqXHR, textStatus, errorThrown) {
            Common.Alert.Error(errorThrown);
            App.unblockUI("#_divbodyFavoriteMenu");
        });
    },
    DeleteFavMenu: function (menu, app) {
        var params = {
            mstMenuID: menu,
            ApplicationID: app
        }
        $.ajax({
            url: "/api/dashboardNavigationMenu/deleteFavoriteMenu",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        })
            .done(function (data, textStatus, jqXHR) {
                DashboardNavigationMenu.FavoriteMenuList();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                Common.Alert.Error(errorThrown);
                DashboardNavigationMenu.FavoriteMenuList();
            })
    },
    CreateFavMenu: function (menu, app) {
        var params = {
            mstMenuID: menu,
            ApplicationID: app
        }
        $.ajax({
            url: "/api/dashboardNavigationMenu/createFavoriteMenu",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(params),
            cache: false
        }).done(function (data, textStatus, jqXHR) {
            DashboardNavigationMenu.FavoriteMenuList();
        }).fail(function (jqXHR, textStatus, errorThrown) {
            DashboardNavigationMenu.FavoriteMenuList();
        })
    }
}

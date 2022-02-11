﻿(function ($) {
    $.extend({
        bb_notify_checkPermission: function (obj, method, requestPermission) {
            if ((!window.Notification && !navigator.mozNotification) || !window.FileReader || !window.history.pushState) {
                console.warn("Your browser does not support all features of this API");
                if (obj !== null) obj.invokeMethodAsync(method, 'false');
            } else if (Notification.permission === "granted") {
                if (obj !== null) obj.invokeMethodAsync(method, 'true');
            } else if (requestPermission && (Notification.permission !== 'denied' || Notification.permission === "default")) {
                Notification.requestPermission(function (permission) {
                    if (permission === "granted") {
                        if (obj !== null) obj.invokeMethodAsync(method, 'true');
                    } else {
                        if (obj !== null) obj.invokeMethodAsync(method, 'false');
                    }
                });
            }
            return true;
        },
        bb_notify_display: function (obj, method, model, onclickmethod) {
            $.bb_notify_checkPermission(null, null, true); 
            if (model.title !== null) {
                var options = {};
                if (model.message !== null)  options.body = model.message.substr(0, 250);
                if (model.icon !== null) options.icon = model.icon;
                if (model.silent !== null) options.silent = model.silent;
                if (model.sound !== null) options.sound = model.sound;
                var notification = new Notification(model.title.substr(0, 100), options);
                if (obj !== null && onclickmethod !== null) {
                    notification.onclick = function (event) {
                        event.preventDefault(); 
                        obj.invokeMethodAsync(onclickmethod, 'true');
                    }
                }
                if (obj !== null) obj.invokeMethodAsync(method, 'true');
                return true;
            }
            if (obj !== null) obj.invokeMethodAsync(method, 'false');
            return true;
        }
    });
})(jQuery);


var TelegramWebAppPlugins =
{
    _GetTelegramInitData: function () {
        var initDataStr = Telegram.WebApp.initData;
        //Get size of the string
        var bufferSize = lengthBytesUTF8(initDataStr) + 1;
        //Allocate memory space
        var buffer = _malloc(bufferSize);
        //Copy old data to the new one then return it
        if (typeof stringToUTF8 !== 'undefined') {
            stringToUTF8(initDataStr, buffer, bufferSize);
        }
        else if (typeof writeStringToMemory !== 'undefined') {
            writeStringToMemory(initDataStr, buffer);
        }
        return buffer;
    },

    _OpenLink: function (link) {
        var url = UTF8ToString(link);
        if (url.startsWith("https://t.me")) {
            Telegram.WebApp.openTelegramLink(url);
        }
        else {
            Telegram.WebApp.openLink(UTF8ToString(link));
        }
    },

    _OpenInvoice: function (taskId, invoice) {
        Telegram.WebApp.openInvoice(UTF8ToString(invoice), status => {
            if (status == "paid") {
                UnityTaskCallBack(taskId, true, status);
            }
            else {
                UnityTaskCallBack(taskId, false, status);
            }
        });
    },

    _CopyToClipboard: function (text) {
        ClipboardJS.copy(UTF8ToString(text));
    },

    _RegisterVisibilityChangeEvent: function () {
        document.addEventListener("visibilitychange", function () {
            SendMessage("GameplayInterrupt", "OnVisibilityChange", document.visibilityState);
        });
    },

    _SetLoadingProgress: function (progress) {
        var loadingBar = document.querySelector("#unity-loading-fg");
        loadingBar.style.width = 100 * progress + "%";

        if (progress == 1) {
            var loadingContainer = document.querySelector("#unity-loading-container");
            loadingContainer.classList.add("finished");

            if (window.eruda) {
                eruda.destroy();
            }
        }
    },

    _SetLoadingText: function (text) {
        var loadingText = document.querySelector("#unity-loading-text");
        loadingText.innerHTML = UTF8ToString(text);
    },

    _ShareToStory: function (mediaUrl, text, widgetUrl, widgetName) {
        function toString(unityString) {
            if (typeof UTF8ToString !== 'undefined') {
                return UTF8ToString(unityString);
            }

            if (typeof Pointer_stringify !== 'undefined') {
                return Pointer_stringify(unityString);
            }
            return unityString;
        }
        const strMediaUrl = toString(mediaUrl);
        const strText = toString(text);
        const strWidgetUrl = toString(widgetUrl);
        const strWidgetName = toString(widgetName);
        const params = {
            text: strText,
            widget_link: {
                url: strWidgetUrl,
                name: strWidgetName
            }
        };

        try {
            Telegram.WebApp.shareToStory(strMediaUrl, params);
            return true;
        }
        catch {
            return false;
        }
    },

    _Reload: function () {
        location.reload();
    },

    _IsProduction: function () {
        return window.location.host.indexOf("client.pocketwaifu.io") > -1;
    },

    _IsMobile: function () {
            var platform = Telegram.WebApp.platform;
            return platform === "android" || platform === "ios";
    },
    
    _Platform: function () {
        var platform = Telegram.WebApp.platform;
        return platform;
    },
   
};
mergeInto(LibraryManager.library, TelegramWebAppPlugins);

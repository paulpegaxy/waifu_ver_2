var Bridge = {
	postMessage: function (data) {
		window.postMessage(UTF8ToString(data), "*");
	},

	walletConnect: function () {
		var jsBridge = window.jsBridge;
		jsBridge.connect(function (response) {
			SendMessage("Bridge", "OnMessage", response);
		});
	},

	walletDisconnect: function () {
		var jsBridge = window.jsBridge;
		jsBridge.disconnect(function (response) {
			SendMessage("Bridge", "OnMessage", response);
		});
	},

	walletSendTransaction: function (data) {
		var jsBridge = window.jsBridge;
		jsBridge.buy(UTF8ToString(data), function (response) {
			SendMessage("Bridge", "OnMessage", response);
		});
	},

	isWalletConnected: function () {
		var jsBridge = window.jsBridge;
		if (jsBridge) {
			return jsBridge.isConnected();
		}
		return false;
	},

	getUserData: function () {
		var str = decodeURIComponent(Telegram.WebApp.initData);
		var bufferSize = lengthBytesUTF8(str) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(str, buffer, bufferSize);

		return buffer;
	}
};

mergeInto(LibraryManager.library, Bridge);
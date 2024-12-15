var WebGLInput = {
    $instances: [],
    WebGLInputInit : function() {
        // use WebAssembly.Table : makeDynCall
        // when enable. dynCall is undefined
        if(typeof dynCall === "undefined")
        {
            // make Runtime.dynCall to undefined
            Runtime = { dynCall : undefined }
        }
        else
        {
            // Remove the `Runtime` object from "v1.37.27: 12/24/2017"
            // if Runtime not defined. create and add functon!!
            if(typeof Runtime === "undefined") Runtime = { dynCall : dynCall }
        }
    },
      WebGLInputCreate: function (canvasId, x, y, width, height, fontsize, text, placeholder, isMultiLine, isPassword, isHidden, isMobile) {
    
            var container = document.getElementById(UTF8ToString(canvasId));
            var canvas = container.getElementsByTagName('canvas')[0];
    
            // if container is null and have canvas
            if (!container && canvas)
            {
                // set the container to canvas.parentNode
                container = canvas.parentNode;
            }
            const input = document.createElement(isMultiLine?"textarea":"input");
            input.classList.add("input-handler")
            
            const platform = window.Telegram.WebApp.platform;
            if (platform === 'ios') {
                input.classList.add("ios");
            } else if (platform === 'android') {
                input.classList.add("android");
            }
            
            if(isMobile){
                input.classList.add("mobile-input");
            } else {
                input.classList.add("desktop-input");
            }
            if(isMultiLine) {
                input.classList.add("multiline-input")
            }
            if(isHidden) {
                input.classList.add("hidden")
            }
    
            input.spellcheck = false;
            input.value = UTF8ToString(text);
            input.placeholder = UTF8ToString(placeholder);
            
            if(isPassword){
                input.type = 'password';
            }
    
            if(isMobile) {
                document.body.appendChild(input);
            } else {
                container.appendChild(input);
            }
            return instances.push(input) - 1;
        },
    WebGLInputEnterSubmit: function(id, falg,cb){
        var input = instances[id];
        // for enter key
        input.addEventListener('keydown', function(e) {
            if ((e.which && e.which === 13) || (e.keyCode && e.keyCode === 13)) {
                if(falg)
                {
                    e.preventDefault();
                    (!!Runtime.dynCall) ? Runtime.dynCall("vi", cb, [id]) : {{{ makeDynCall("vi", "cb") }}}(id);
                    input.blur();
                }
            }
        });
    },
    WebGLInputTab:function(id, cb) {
        var input = instances[id];
        // for tab key
        input.addEventListener('keydown', function (e) {
            if ((e.which && e.which === 9) || (e.keyCode && e.keyCode === 9)) {
                e.preventDefault();

                // if enable tab text
                if(input.enableTabText){
                    var val = input.value;
                    var start = input.selectionStart;
                    var end = input.selectionEnd;
                    input.value = val.substr(0, start) + '\t' + val.substr(end, val.length);
                    input.setSelectionRange(start + 1, start + 1);
                    input.oninput();	// call oninput to exe ValueChange function!!
                } else {
                    (!!Runtime.dynCall) ? Runtime.dynCall("vii", cb, [id, e.shiftKey ? -1 : 1]) : {{{ makeDynCall("vii", "cb") }}}(id, e.shiftKey ? -1 : 1);
                }
            }
        });
    },
    WebGLInputFocus: function(id){
        var input = instances[id];
        input.focus();
    },
    WebGLInputOnFocus: function (id, cb) {
        var input = instances[id];
        input.onfocus = function () {
            (!!Runtime.dynCall) ? Runtime.dynCall("vi", cb, [id]) : {{{ makeDynCall("vi", "cb") }}}(id);
        };
    },
    WebGLInputOnBlur: function (id, cb) {
        var input = instances[id];
        input.onblur = function () {
            (!!Runtime.dynCall) ? Runtime.dynCall("vi", cb, [id]) : {{{ makeDynCall("vi", "cb") }}}(id);
        };
    },
    WebGLInputIsFocus: function (id) {
        return instances[id] === document.activeElement;
    },
    WebGLInputOnValueChange:function(id, cb){
        var input = instances[id];
        input.oninput = function () {
            var returnStr = input.value;
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            (!!Runtime.dynCall) ? Runtime.dynCall("vii", cb, [id, buffer]) : {{{ makeDynCall("vii", "cb") }}}(id, buffer);
        };
    },
    WebGLInputOnEditEnd:function(id, cb){
        var input = instances[id];
        input.onchange = function () {
            var returnStr = input.value;
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            (!!Runtime.dynCall) ? Runtime.dynCall("vii", cb, [id, buffer]) : {{{ makeDynCall("vii", "cb") }}}(id, buffer);
        };
    },
    WebGLInputOnKeyboardEvent:function(id, cb){
        var input = instances[id];
        var func = function(mode, e) {
            if (e instanceof KeyboardEvent){
                var bufferSize = lengthBytesUTF8(e.key) + 1;
                var key = _malloc(bufferSize);
                stringToUTF8(e.key, key, bufferSize);
                var code = e.code;
                var shift = e.shiftKey ? 1 : 0;
                var ctrl = e.ctrlKey ? 1 : 0;
                var alt = e.altKey ? 1 : 0;
                (!!Runtime.dynCall) ? Runtime.dynCall("viiiiiii", cb, [id, mode, key, code, shift, ctrl, alt]) : {{{ makeDynCall("viiiiiii", "cb") }}}(id, mode, key, code, shift, ctrl, alt);
            }
        }
        input.addEventListener('keydown', function(e) { func(1, e); });
        input.addEventListener('keyup', function(e) { func(2, e); });
    },
    WebGLInputSelectionStart:function(id){
        var input = instances[id];
        return input.selectionStart;
    },
    WebGLInputSelectionEnd:function(id){
        var input = instances[id];
        return input.selectionEnd;
    },
    WebGLInputSelectionDirection:function(id){
        var input = instances[id];
        return (input.selectionDirection == "backward")?-1:1;
    },
    WebGLInputSetSelectionRange:function(id, start, end){
        var input = instances[id];
        input.setSelectionRange(start, end);
    },
    WebGLInputMaxLength:function(id, maxlength){
        var input = instances[id];
        input.maxLength = maxlength;
    },
    WebGLInputText:function(id, text){
        var input = instances[id];
        input.value = UTF8ToString(text);
    },
    WebGLInputDelete:function(id){
        var input = instances[id];
        input.parentNode.removeChild(input);
        instances[id] = null;
    },
    WebGLInputEnableTabText:function(id, enable) {
        var input = instances[id];
        input.enableTabText = enable;
    },
    WebGLInputForceBlur:function(id) {
        // console.log('WebGLInputForceBlur');
        var input = document.querySelector('.input-handler');      
       // console.log('input ne: ', input);
        input && input.blur();
        
        setTimeout(() => {
           // console.log("Fix body"); // Log statement
            const body = document.querySelector("#main-body")
            body.style.marginTop = "100px"
            if (!body) { return }
            setTimeout(() => {
                body.style.marginTop = "0px"
            }, 100);
         }, 1);
    },
}

autoAddDeps(WebGLInput, '$instances');
mergeInto(LibraryManager.library, WebGLInput);

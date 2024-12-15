var TONConnectPlugins =
{
    _ConnectWallet: function (taskId) {
        tonConnectUI.connectWallet()
            .then(data => UnityTaskCallBack(taskId, true, data))
            .catch(err => UnityTaskCallBack(taskId, false, err));
    },
    _Disconnect: function (taskId) {
        tonConnectUI.disconnect()
            .then(data => UnityTaskCallBack(taskId, true, data))
            .catch(err => UnityTaskCallBack(taskId, false, err));
    },
    _GetWallet: function () {
        if (tonConnectUI.wallet !== null) tonConnectUI.wallet.account.userFriendlyAddress = new TonWeb.Address(tonConnectUI.wallet.account.address).toString(true, false, false, tonConnectUI.wallet.account.chain !== '-239');
        var walletDataStr = JSON.stringify(tonConnectUI.wallet);
        var bufferSize = lengthBytesUTF8(walletDataStr) + 1;
        var buffer = _malloc(bufferSize);
        if (typeof stringToUTF8 !== 'undefined') {
            stringToUTF8(walletDataStr, buffer, bufferSize);
        }
        else if (typeof writeStringToMemory !== 'undefined') {
            writeStringToMemory(walletDataStr, buffer);
        }
        return buffer;
    },
    _SendTON: async function (taskId, address, amount, payload, validUntil) {
        function toString(unityString) {
            if (typeof UTF8ToString !== 'undefined') {
                return UTF8ToString(unityString);
            }

            if (typeof Pointer_stringify !== 'undefined') {
                return Pointer_stringify(unityString);
            }
            return unityString;
        }

        const strAddress = toString(address);
        const strAmount = toString(amount);
        const strPayload = toString(payload);
        try {
            if (!tonConnectUI.connected) {
                await tonConnectUI.connectWallet();
            }
            const cell = new TonWeb.boc.Cell();
            cell.bits.writeUint(0, 32);
            cell.bits.writeString(strPayload);
            const base64payload = TonWeb.utils.bytesToBase64(await cell.toBoc());
            const transaction = {
                messages: [
                    {
                        address: strAddress, // destination address
                        amount: TonWeb.utils.toNano(strAmount).toString(), //Toncoin in nanotons
                        payload: base64payload //Base 64 payload
                    }
                ]
            };
            if (validUntil != 0) transaction.validUntil = validUntil;
            const boc = await tonConnectUI.sendTransaction(transaction);
            const bocData = TonWeb.utils.base64ToBytes(boc.boc);
            const cellResp = TonWeb.boc.Cell.oneFromBoc(bocData);
            const hash = TonWeb.utils.bytesToBase64(await cellResp.hash());
            UnityTaskCallBack(taskId, true, hash);
        } catch (err) {
            UnityTaskCallBack(taskId, false, err);
        }
    },
    _ClaimTON: async function (taskId, data) {
        function toString(unityString) {
            if (typeof UTF8ToString !== 'undefined') {
                return UTF8ToString(unityString);
            }

            if (typeof Pointer_stringify !== 'undefined') {
                return Pointer_stringify(unityString);
            }
            return unityString;
        }

        try {
            const strData = toString(data);
            const jsonData = JSON.parse(strData);
            const cell = new TonWeb.boc.Cell();
            cell.bits.writeUint(802178298, 32); //claim op code

            const signatureCell = new TonWeb.boc.Cell();
            const bytesSignature = TonWeb.utils.base64ToBytes(jsonData.signature);
            signatureCell.bits.writeBytes(bytesSignature);
            cell.refs.push(signatureCell); //Signature

            const address = new TonWeb.utils.Address(jsonData.to_address);
            cell.bits.writeAddress(address); //Receiver address

            const nanoAmount = TonWeb.utils.toNano(jsonData.amount.toString());
            cell.bits.writeCoins(nanoAmount); //Amount
            cell.bits.writeUint(jsonData.deadline, 32); //Deadline
            cell.bits.writeUint(Number(jsonData.order_id), 64); //txId

            const payload = TonWeb.utils.bytesToBase64(await cell.toBoc());
            const transaction = {
                messages: [
                    {
                        address: jsonData.contract_address,
                        amount: TonWeb.utils.toNano('0.01').toString(),
                        payload: payload,
                    },
                ],
            };

            transaction.validUntil = Math.floor(Date.now() / 1000) + 3600;
            const result = await tonConnectUI.sendTransaction(transaction);
            const msgBody = TonWeb.utils.base64ToBytes(result.boc);
            const msgCell = TonWeb.boc.Cell.oneFromBoc(msgBody);
            const hash = TonWeb.utils.bytesToBase64(await msgCell.hash());
            UnityTaskCallBack(taskId, true, hash);
        } catch (err) {
            UnityTaskCallBack(taskId, false, err);
        }
    },
    _SendJetton: async function (taskId, data) {
        function toString(unityString) {
            if (typeof UTF8ToString !== 'undefined') {
                return UTF8ToString(unityString);
            }

            if (typeof Pointer_stringify !== 'undefined') {
                return Pointer_stringify(unityString);
            }
            return unityString;
        }
        function convertAmount(amount, decimals) {
            let comps = amount.toString().split(".");
            let whole = comps[0];
            let fraction = comps[1].substring(0, decimals);
            if (!whole) whole = "0";
            if (!fraction) fraction = "0";
            while (fraction.length < decimals) fraction += "0";
            whole = new TonWeb.utils.BN(whole);
            fraction = new TonWeb.utils.BN(fraction);
            const multiplier = new TonWeb.utils.BN(10).pow(new TonWeb.utils.BN(decimals));
            return whole.mul(multiplier).add(fraction);
        }

        try {
            if (!tonConnectUI.connected) {
                await tonConnectUI.connectWallet();
            }
            const strData = toString(data);
            const jsonData = JSON.parse(strData);
            const sourceAddress = new TonWeb.utils.Address(tonConnectUI.wallet.account.address);
            const destinationAddress = new TonWeb.utils.Address(jsonData.destinationAddress);
            const comment = new Uint8Array([... new Uint8Array(4), ... new TextEncoder().encode(jsonData.comment)]);
            const cell = new TonWeb.boc.Cell();
            cell.bits.writeUint(0xf8a7ea5, 32); // opcode for jetton transfer
            cell.bits.writeUint(0, 64); // query id
            cell.bits.writeCoins(convertAmount(jsonData.amount, jsonData.token_decimals)); // store jetton amount
            cell.bits.writeAddress(destinationAddress); // TON wallet destination address
            cell.bits.writeAddress(sourceAddress); // response excess destination
            cell.bits.writeBit(false); // no custom payload
            cell.bits.writeCoins(TonWeb.utils.toNano('0')); // forward amount (if >0, will send notification message)
            cell.bits.writeBit(false); // we store forwardPayload as a reference
            cell.bits.writeBytes(comment);
            const payload = TonWeb.utils.bytesToBase64(await cell.toBoc());
            const transaction = {
                validUntil: Math.floor(Date.now() / 1000) + 360,
                messages: [
                    {
                        address: jsonData.jettonWallet,
                        amount: TonWeb.utils.toNano('0.05').toString(),
                        payload: payload,
                    },
                ],
            };
            const result = await tonConnectUI.sendTransaction(transaction);
            const msgBody = TonWeb.utils.base64ToBytes(result.boc);
            const msgCell = TonWeb.boc.Cell.oneFromBoc(msgBody);
            const hash = TonWeb.utils.bytesToBase64(await msgCell.hash());
            UnityTaskCallBack(taskId, true, hash);
        } catch (err) {
            UnityTaskCallBack(taskId, false, err);
        }
    }
};
mergeInto(LibraryManager.library, TONConnectPlugins);

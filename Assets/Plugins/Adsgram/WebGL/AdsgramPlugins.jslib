var AdsgramPlugins =
{
    _AdsgramShow: function (taskId, blockId) {
        const AdController = Adsgram.init({ blockId: `${blockId}` });
        AdController.show().then((result) => {
            UnityTaskCallBack(taskId, true, result);
        }).catch((result) => {
            UnityTaskCallBack(taskId, false, result);
        })
    }
};
mergeInto(LibraryManager.library, AdsgramPlugins);

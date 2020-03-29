
bindFormMethod = function () {
    $("#file_uploadpic").uploadify({
        'debug': false,
        'preventCaching': false,
        'auto': true,
        'method': 'post',
        'buttonImage': '../Content/Images/uploadpic.png',
        'formData': {
            'folder': '../ELoad/Picture'
        },
        'swf': "../Content/uploadify/uploadify.swf",
        'queueID': "uploadfileQueuepic",
        'uploader': '../Upload/UploadList',
        'multi': false,
        'width': '80',
        'height': '20',
        'overrideEvents': ['onDialogClose'],
        'fileTypeDesc': '请选择.jpg',
        'fileTypeExts': '*.jpg',
        'fileSizeLimit': '1MB',
        'removeTimeout': 1,
        'onSelectError': function (file, errorCode, errorMsg) {
            switch (errorCode) {
                case -100:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 上传的文件数量已经超出系统限制！", 2000);
                    break;
                case -110:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 大小超出系统限制的！", 2000);
                    break;
                case -120:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 大小异常！", 2000);
                    break;
                case -130:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 类型不正确！", 2000);
                    break;
            }
        }, 'onFallback': function () {
            Easy.centerShow("系统消息", "您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。", 2000);
        },
        'onUploadSuccess': function (file, data, response) {
            $("#picture").attr("src", data);
            $("#filePathpic").textbox("setValue", data);
        }
    });

    setTimeout(function () {
        $("#StudentIDpic").textbox("setValue", StudentID);
    }, 1);
}
bindFormMethod();
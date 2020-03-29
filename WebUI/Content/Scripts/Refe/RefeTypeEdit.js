//绑定显示基础分类信息的方法
bindSelectRefeTypeInfo = function (refeTypeId) {
    if (refeTypeId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Refe/SelectRefeType", refeTypeId);
            var refetype = JSON.parse(result.Message)[0];
            $("#RefeTypeID").textbox("setValue", refetype.RefeTypeID);
            $("#ModuleName").textbox("setValue", refetype.ModuleName);
            $("#TypeName").textbox("setValue", refetype.TypeName);
            $("#RefeTypeRemark").textbox("setValue", refetype.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

bindSelectRefeTypeInfo(RefeTypeID);
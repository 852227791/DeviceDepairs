//绑定信息明细表单数据
bindRefeFormEvent = function (RefeTypeID) {
    $("#RefeType").combobox({
        url: "../Refe/SelRefeTypeList",
        valueField: "RefeTypeID",
        textField: "TypeName",
        value: RefeTypeID,
        editable: false
    });
}

//绑定显示基础分类信息的方法
bindSelectRefeInfo = function (refeId) {
    if (refeId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Refe/SelectRefe", refeId);
            var refe = JSON.parse(result.Message)[0];
            $("#RefeID").textbox("setValue", refe.RefeID);
            $("#RefeName").textbox("setValue", refe.RefeName);
            $("#RefeType").combobox("setValue", refe.RefeTypeID);
            $("#Value").textbox("setValue", refe.Value);
            $("#Queue").textbox("setValue", refe.Queue);
            $("#RefeRemark").textbox("setValue", refe.Remark.replace("<br />", "\r\n"));
        }, 1);
    }
}

bindRefeFormEvent(RefeTypeID);

bindSelectRefeInfo(RefeID);
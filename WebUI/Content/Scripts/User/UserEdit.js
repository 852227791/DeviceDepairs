//绑定表单
bindFormEvent = function () {
    $("input[name='Sex'][value=3]").prop("checked", true);//默认选中保密
    $("#UserType").combobox({
        url: "../Refe/SelList?RefeTypeID=2",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        editable: false,
        value: 1
    });
}

//绑定显示用户信息的方法
bindSelectUserInfo = function (userId) {
    if (userId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../User/SelectUser", userId);
            var user = JSON.parse(result.Message)[0];
            $("#UserID").textbox("setValue", user.UserID);
            $("#LoginName").textbox("setValue", user.LoginName);
            $("#Name").textbox("setValue", user.Name);
            $("input[name='Sex'][value=" + user.Sex + "]").prop("checked", true);
            $("#Mobile").textbox("setValue", user.Mobile);
            $("#UserType").combobox("setValue", user.UserType);
            $("#Remark").textbox("setValue", user.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../User/CheckLoginName";
    row1.id = "#UserID";
    row1.type = [];
    data.jsontext.push(row1);
    Easy.checkValue(data);
}

//加载表单
bindFormEvent();

bindSelectUserInfo(UserID);//加载数据

//验证登录名是否重复
bindCheckEvent();

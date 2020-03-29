//绑定表单数据
bindFormEvent = function () {
    $("#RoleType").combobox({
        url: "../Refe/SelList?RefeTypeID=2",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: "80",
        editable: false,
        value: 1
    });
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Role/CheckName";
    row1.id = "#RoleID";
    row1.type = [];
    data.jsontext.push(row1);
    Easy.checkValue(data);
}

//绑定显示角色信息的方法
bindSelectRoleInfo = function (roleId) {
    if (roleId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Role/SelectRole", roleId);
            var role = JSON.parse(result.Message)[0];
            $("#RoleID").textbox("setValue", role.RoleID);
            $("#Name").textbox("setValue", role.Name);
            $("#RoleType").combobox("setValue", role.RoleType);
            $("#Description").textbox("setValue", role.Description);
            $("#Remark").textbox("setValue", role.Remark.replace("<br />", "\r\n"));
        }, 1);
    }
}

//加载表单数据
bindFormEvent();

bindSelectRoleInfo(RoleID);//显示角色信息

//验证角色名是否重复
bindCheckEvent();

$.parser.parse("#fSave");//解析
//绑定表单数据
bindFormEvent = function (UserID, RoleID) {
    $("#UserID").textbox("setValue", UserID);
    $("#RoleID").textbox("setValue", RoleID);
    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
    $("#Range").combobox({
        url: "../Refe/SelList?RefeTypeID=4",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        editable: false
    });
}

bindFormEvent(userId, roleId);//表单数据
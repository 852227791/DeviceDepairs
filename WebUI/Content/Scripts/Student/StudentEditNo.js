bindFormEvent = function () {
    $("#noDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        panelWidth: 300,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#noDeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
        }
    });
    $("#noSex").combobox({
        url: "../Refe/SelList?RefeTypeID=3",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false
    });
}

bindSelectInfo = function () {
    if (StudentID != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Student/SelectStudent", StudentID);
            var p = JSON.parse(result.Message)[0];
            $("#noStudentID").textbox("setValue", p.StudentID);
            $("#noDeptID").combotree("setValue", p.DeptID);
            $("#noName").textbox("setValue", p.Name);
            $("#noIDCard").textbox("setValue", p.IDCard);
            $("#noSex").combobox("setValue", p.Sex);
            $("#noMobile").textbox("setValue", p.Mobile);
            $("#noQQ").textbox("setValue", p.QQ);
            $("#noWeChat").textbox("setValue", p.WeChat);
            $("#noAddress").textbox("setValue", p.Address);
            $("#noRemark").textbox("setValue", p.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

bindFormEvent();
bindSelectInfo();


bindFormEvent = function () {
    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        panelWidth:300,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#DeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
        }
    });
    $("#Sex").combobox({
        url: "../Refe/SelList?RefeTypeID=3",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false
    });
}
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Student/CheckIDCardIsRepeat";
    row1.id = "#StudentID";
    row1.type = [];
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Student/CheckIDCardIsStandard";
    row2.id = "#StudentID";
    row2.type = [];
    data.jsontext.push(row2);
    Easy.checkValue(data);
}

bindCheckEvent();
bindFormEvent();
bindSelectInfo = function () {
    if (StudentID != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Student/SelectStudent", StudentID);
            var p = JSON.parse(result.Message)[0];
            $("#StudentID").textbox("setValue", p.StudentID);
            $("#DeptID").combotree("setValue", p.DeptID);
            $("#Name").textbox("setValue", p.Name);
            $("#IDCard").textbox("setValue", p.IDCard);
            $("#Sex").combobox("setValue", p.Sex);
            $("#Mobile").textbox("setValue", p.Mobile);
            $("#QQ").textbox("setValue", p.QQ);
            $("#WeChat").textbox("setValue", p.WeChat);
            $("#Address").textbox("setValue", p.Address);
            $("#Remark").textbox("setValue", p.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}
bindSelectInfo();


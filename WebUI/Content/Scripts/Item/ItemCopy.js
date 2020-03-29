bindFormEvent = function () {
    $("#ToDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 350,
        panelHeight: 400,
        onClick: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#ToDeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);

            }
            else {
                bindToItemCombobox(node.id);
            }

        }
    });

    $("#FromDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 350,
        panelHeight: 400,
        onClick: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#FromDeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);

            }
            else {
                bindFromItemCombobox(node.id);
            }

        }
    });
    //var nodes = $("#itemTree").treegrid("getSelections");
    //var idstring = "";
    //for (var i = 0; i < nodes.length; i++) {
    //    idstring += nodes[i].id+ ",";
    //}
    //idstring = idstring.substring(0,idstring.length-1);
    //$("#FromItemID").val(idstring);
}
bindFormEvent();

bindToItemCombobox = function (deptId) {

    $("#ToItemID").combotree({
        url: "../Item/ItemCombotreeMore",
        queryParams: { DeptID: deptId },
        animate: true,
        lines: true,
        panelWidth: 350,
        panelHeight: 400,
        onSelect: function (node) {
            //var tree = $(this).tree;
            //var isLeaf = tree('isLeaf', node.target);
            //if (!isLeaf) {
            //    $('#ItemID').combotree('clear');
            //    Easy.centerShow("系统消息", "请选择末级节点", 3000);
            //}
            //else {
            //    var studentId = $("#StudentID").textbox('getValue');
            //    checkItemNumEvent(studentId, node.id);
            //}
        }
    });
}
bindFromItemCombobox = function (deptId) {

    $("#FromItemID").combotree({
        url: "../Item/ItemCombotreeMore",
        queryParams: { DeptID: deptId },
        animate: true,
        lines: true,
        onlyLeafCheck: false,
        cascadeCheck: false,
        multiple:true,
        panelWidth: 350,
        panelHeight: 400,
        onSelect: function (node) {
            //var tree = $(this).tree;
            //var isLeaf = tree('isLeaf', node.target);
            //if (!isLeaf) {
            //    $('#ItemID').combotree('clear');
            //    Easy.centerShow("系统消息", "请选择末级节点", 3000);
            //}
            //else {
            //    var studentId = $("#StudentID").textbox('getValue');
            //    checkItemNumEvent(studentId, node.id);
            //}
        }
    });
}
bindFormEvent = function () {
    $("#editDetailID").combotree({
        url: "../Detail/GetDetailCommonbox",
        queryParams: { Status: "1" },
        animate: true,
        lines: true,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#editDetailID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
        }
    });
}

bindSelectInfo = function () {
    var sorderid = $("#grid").datagrid("getSelected").sOrderID;
    if (sorderid != "") {
        setTimeout(function () {
            var result1 = Easy.bindSelectInfo("../sOrder/SelectsOrder", sorderid);
            var sorder = JSON.parse(result1.Message)[0];
            $("#editsOrderID").textbox("setValue", sorder.sOrderID);
            $("#editDetailID").combotree("setValue", sorder.DetailID);
            $("#editShouldMoney").numberspinner("setValue", sorder.ShouldMoney);
            $("#editLimitTime").datebox("setValue", Easy.bindSetTimeFormatEvent(sorder.LimitTime));
        }, 1);
    }
}

bindFormEvent();

bindSelectInfo();

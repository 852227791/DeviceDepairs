Easy.UpLoadFile();
bindFormMethod = function () {

    $("#upDeptId").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1,2" },
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
                bindupItemCombobox(node.id);
            }

        }
    });
};
bindFormMethod();

bindupItemCombobox = function (deptId) {
    $("#upItemId").combotree({
        url: "../Item/ItemCombotreeMore",
        queryParams: { DeptID: deptId },
        animate: true,
        lines: true,
        panelWidth: 350,
        panelHeight: 400
    });
}
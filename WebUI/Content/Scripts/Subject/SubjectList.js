var subjectId = "0";
firstFunction = function () {
    Easy.bindSaveButtonClickEvent("#editsubject", "#fSave", "../Subject/GetSubjectEdit", "#btnSave", "4", "#subtree", menuId, "1", "#SubjectID");
    initTreeGrid();
};

initTreeGrid = function () {
    $('#subtree').treegrid({
        url: '../Subject/GetSubjectList',
        idField: 'id',
        treeField: 'text',
        columns: [[
            { field: 'id', width: 180, hidden: true },
            { field: 'text', title: '科目名称', width: 180 },
            { field: 'EnglishName', title: '首字母', width: 80 },
            {
                field: 'Status', title: '状态', width: 80,
                formatter: function (value, rowData) {
                    if (value == "停用")
                        return '<span style="color:#ff0000;">' + value + '</span>';
                    else
                        return value;
                }
            },
            { field: 'Remark', title: '备注', width: 250 }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1")
    });

}
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        subjectId = "0";
        Easy.OpenDialogEvent("#editsubject", "编辑会计科目", 600, 380, "../Subject/SubjectEdit", "#editsubject-buttons");
    }
};
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var node = $('#subtree').treegrid("getSelected");//选中的所有ID
        if (node === null) {
            Easy.centerShow("系统消息", "请选择会计科目", 3000);
            return false;
        }
        if (node.id === 1) {
            Easy.centerShow("系统消息", "此节点不能操作", 3000);
            return false;
        }
        subjectId = node.id;
        Easy.OpenDialogEvent("#editsubject", "编辑会计科目", 600, 380, "../Subject/SubjectEdit", "#editsubject-buttons");
    }
};
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
};
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
};

bindUpdateStatus = function (confirmstr, status) {
    var node = $('#subtree').treegrid("getSelected");
   
    if (node === null) {
        Easy.centerShow("系统消息", "请选择会计科目", 3000);
        return false;
    }
   
    if (node.id===1) {
        Easy.centerShow("系统消息", "此节点不能操作", 3000);
        return false;
    }
    Easy.bindUpdateValue(confirmstr, status, "../Subject/GetUpdateStatus", node.id, "3", "#subtree");
}

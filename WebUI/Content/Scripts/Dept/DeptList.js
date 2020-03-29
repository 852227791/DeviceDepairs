var DeptID = "0";//定义ID
firstFunction = function () {
    bindBtnLoadEvent();//加载按钮
    initTree();//加载树

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editDept", "#fSave", "../Dept/GetDeptEdit", "#btnSave", "2", "#deptTree", menuId, "1", "#DeptID");
}

//绑定按钮加载
bindBtnLoadEvent = function () {
    var btn = Easy.loadAloneToolbar(menuId, "1");
    if (btn != "") {
        $("#infoMain").layout("add", {
            region: "north",
            border: false,
            height: 30,
            content: btn
        });
    }
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        DeptID = "0";
        Easy.OpenDialogEvent("#editDept", "编辑部门", 650, 280, "../Dept/DeptEdit", "#editDept-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var nodes = $("#deptTree").tree("getSelected");//选中的所有ID
        //验证是否选中行
        if (Easy.checkNode(nodes, "部门")) {
            if (nodes.id != "1") {
                DeptID = nodes.id;
                Easy.OpenDialogEvent("#editDept", "编辑部门", 650, 280, "../Dept/DeptEdit", "#editDept-buttons");
            }
            else {
                Easy.centerShow("系统消息", "此信息不可修改", 3000);
            }
        }
    }
}

//启用
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateDeptStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateDeptStatus("停用", "2");
    }
}

//加载树
initTree = function () {
    $("#deptTree").tree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1,2" },
        animate: true,
        lines: true,
        onClick: function (node) {
            if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
                bindViewDeptInfo(node.id);
            }
        }
    });
}

//查看部门信息的方法
bindViewDeptInfo = function (DeptID) {
    var result = Easy.bindSelectInfo("../Dept/SelectViewDept", DeptID);
    $("#txtName").html(result.Data[0].Name);
    $("#txtParentName").html(result.Data[0].ParentName);
    $("#txtShortName").html(result.Data[0].ShortName);
    $("#txtCode").html(result.Data[0].Code);
    $("#txtQueue").html(result.Data[0].Queue);
    $("#txtRemark").html(result.Data[0].Remark);
}

//绑定更新状态方法
bindUpdateDeptStatus = function (confirmstr, status) {
    var nodes = $("#deptTree").tree("getSelected");//选中的所有ID
    //验证是否选中行
    if (Easy.checkNode(nodes, "部门")) {
        $.ajax({
            type: "post",
            url: "../Dept/CheckIsHandle",
            async: true,
            data: { DeptID: nodes.id, Status: status },
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    //修改状态
                    Easy.bindUpdateValue(confirmstr, status, "../Dept/GetUpdateDeptStatus", nodes.id, "2", "#deptTree");
                }
                else {
                    Easy.centerShow("系统消息", "" + result.Message + "", 3000);
                }
            },
            error: function () {
                Easy.centerShow("系统消息", "操作失败", 3000);
            }
        });
    }
}
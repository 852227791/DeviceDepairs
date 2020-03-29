var DetailID = "0";//定义ID
firstFunction = function () {
    bindBtnLoadEvent();//加载按钮
    initTree();//加载树

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editDetail", "#fSave", "../Detail/GetDetailEdit", "#btnSave", "2", "#detailTree", menuId, "1", "#DetailID");
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
        DetailID = "0";
        Easy.OpenDialogEvent("#editDetail", "添加收费类别", 650, 280, "../Detail/DetailEdit", "#editDetail-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var nodes = $("#detailTree").tree("getSelected");//选中的所有ID
        //验证是否选中行
        if (Easy.checkNode(nodes, "收费类别")) {
            if (nodes.id != "6") {
                DetailID = nodes.id;
                Easy.OpenDialogEvent("#editDetail", "编辑收费类别", 650, 280, "../Detail/DetailEdit", "#editDetail-buttons");
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
        bindUpdateDetailStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateDetailStatus("停用", "2");
    }
}

//加载树
initTree = function () {
    $("#detailTree").tree({
        url: "../Detail/GetDetailTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1,2" },
        animate: true,
        lines: true,
        onClick: function (node) {
            if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
                bindViewDetailInfo(node);
            }
        }
    });
}

//查看部门信息的方法
bindViewDetailInfo = function (node) {
    var parent = $("#detailTree").tree("getParent", node.target);
    if (parent != null) {
        $("#txtParentName").html(parent.text);
    }
    else {
        $("#txtParentName").html("");
    }
    $("#txtName").html(node.text);
    $("#txtEnglistName").html(node.EnglishName);
    $("#txtRemark").html(node.Remark);
    $("#txtSubject").html(node.SubName);
}

//绑定更新状态方法
bindUpdateDetailStatus = function (confirmstr, status) {
    var nodes = $("#detailTree").tree("getSelected");//选中的所有ID
    //验证是否选中行
    if (Easy.checkNode(nodes, "收费类别")) {
        $.ajax({
            type: "post",
            url: "../Detail/CheckIsHandle",
            async: true,
            data: { DetailID: nodes.id, Status: status },
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    //修改状态
                    Easy.bindUpdateValue(confirmstr, status, "../Detail/GetUpdateDetailStatus", nodes.id, "2", "#detailTree");
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
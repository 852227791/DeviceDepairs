var DeptID = "0";
var ItemID = "0";//定义ID
var ItemDetailID = "0";
var sItemsProfessionID = "0";
var sItemsGiveID = "0";
var DeptData = "[]";
var TreeData = "[]";
var itemId = [];
var DiscountPlanID = "0";
firstFunction = function () {
    initDeptTree();
    initTree(0);//加载树
    initItemDetail({});
    bindCheckEvent();//验证名称是否重复
    // bindBtnLoadEvent();
    initProfessionGrid();
    initGiveGrid();

    Easy.bindSaveButtonClickEvent("#copyItem", "#fCopy", "../Item/GetItemCopy", "#btnCopy", "4", "#itemTree", menuId, "1", "#ToDeptID");
    bindSaveItem();
    //Easy.bindSaveButtonClickEvent("#editItem", "#fSave", "../Item/GetItemEdit", "#btnSave", "4", "#itemTree", menuId, "1", "#ItemID");
    Easy.bindSaveButtonClickEvent("#editDetailItem", "#fSave1", "../ItemDetail/GetItemDetailEdit", "#btnSaveItemDetail", "1", "#grid", menuId, "2", "#ItemDetailID");
    Easy.bindSaveButtonClickEvent("#editItemPro", "#fSave_Pro", "../sItemsProfession/GetItemsProfessionEdit", "#btnSave_Pro", "1", "#gridProfession", menuId, "2", "#sItemsProfessionID");
    Easy.bindSaveButtonClickEvent("#editItemGive", "#fSave_Give", "../sItemsGive/GetsItemsGiveEdit", "#btnSave_Give", "1", "#gridGive", menuId, "2", "#sItemsGiveID");

    bindSaveandContinue();
    bindSaveItemDetailandContinue();
    bindSaveItemUploadMehod();
    bindSaveAddDiscountPlan();
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        var nodes = $("#deptTree").tree("getSelected");
        if (Easy.checkTreeGridRow(nodes, "校区")) {
            DeptID = nodes.id;
            ItemID = "0";
            Easy.OpenDialogEvent("#editItem", "编辑收费项", 650, 420, "../Item/ItemEdit", "#editItem-buttons");
        }
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var nodes1 = $("#deptTree").tree("getSelected");
        var nodes2 = $("#itemTree").treegrid("getSelected");
        if (Easy.checkTreeGridRow(nodes2, "证书")) {
            if (nodes2.id != "-1") {
                DeptID = nodes1.id;
                ItemID = nodes2.id;
                Easy.OpenDialogEvent("#editItem", "编辑收费项", 650, 420, "../Item/ItemEdit", "#editItem-buttons");
            }
            else {
                Easy.centerShow("系统消息", "此信息不可修改", 3000);
            }
        }
    }
}

viewDisable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "viewDisable")) {
        var node = $("#deptTree").tree("getSelected");
        if (node === null) {
            Easy.centerShow("系统消息", "请选择主体", 3000);
            return false;
        }
        $('#itemTree').treegrid({
            url: "../Item/GetItemTree",
            queryParams: { Status: "1,2", DeptID: node.id }
        });
    }
};
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
initDeptTree = function () {
    $("#deptTree").tree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        onSelect: function (node) {
            if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
                initTree(node.id);
                DeptID = node.id;
            }
            itemId = [];
            $('#itemTree').treegrid("uncheckAll");
            //刷新项目设置
            $("#grid").datagrid("loadData", []);
            $("#gridProfession").datagrid("loadData", []);
            $("#gridGive").datagrid("loadData", []);
        }
    });

}



//绑定更新状态方法
bindUpdateDeptStatus = function (confirmstr, status) {
    var nodes = $("#itemTree").treegrid("getSelections");//选中的所有ID
    if (nodes.length > 1) {
        Easy.centerShow("系统消息", "只能选择1条信息", 2000);
        return false;
    }
    //验证是否选中行
    if (Easy.checkNode(nodes[0], "收费项")) {
        $.ajax({
            type: "post",
            url: "../Item/CheckIsHandle",
            async: true,
            data: { Item: nodes[0].id, Status: status },
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    //修改状态
                    Easy.bindUpdateValue(confirmstr, status, "../Item/GetUpdateStatus", nodes[0].id, "3", "#itemTree");
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



//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Item/CheckItemName";
    row1.id = "#ItemID";
    row1.type = [{}, {}, {}];
    row1.type[0].typeId = "#DeptID";
    row1.type[0].type = "3";
    row1.type[1].typeId = "#Sort";
    row1.type[1].type = "1";
    row1.type[2].typeId = "#ParentID";
    row1.type[2].type = "2";
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Item/CheckItemIDIsParentID";
    row2.id = "#ItemID";
    row2.type = [{}];
    row2.type[0].typeId = "#ParentID";
    row2.type[0].type = "3";
    data.jsontext.push(row2);
    var row3 = {};
    row3.url = "../ItemDetail/CheckItemDetailName";
    row3.id = "#ItemDetailID";
    row3.type = [{}];
    row3.type[0].typeId = "#ItemID1";
    row3.type[0].type = "3";
    data.jsontext.push(row3);
    Easy.checkValue(data);
}

adddetail = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "adddetail")) {
        var nodes = $("#itemTree").treegrid("getSelected");
        if (Easy.checkTreeGridRow(nodes, "收费项目")) {
            //  var chuild = $('#itemTree').treegrid("getChildren", nodes.id);
            ItemDetailID = "0";
            ItemID = nodes.id;
            Easy.OpenDialogEvent("#editDetailItem", "编辑收费项目明细", 500, 420, "../ItemDetail/ItemDetailEdit", "#editDetailItem-buttons");
            //if (chuild.length === 0) {

            //}
            //else {
            //    Easy.centerShow("系统消息", "此项目不能被选择", 1000);
            //    $('#itemTree').treegrid("unselect", nodes.id);
            //}
        }
    }
}
editdetail = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "editdetail")) {
        var rows = $("#grid").datagrid("getSelections");
        if (Easy.checkRow(rows, "收费项目明细")) {
            ItemDetailID = rows[0].ItemDetailID;
            Easy.OpenDialogEvent("#editDetailItem", "编辑收费项目明细", 500, 420, "../ItemDetail/ItemDetailEdit", "#editDetailItem-buttons");
        }
    }

}
enabledetail = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "enabledetail")) {
        bindUpdateStatus("启用", "1");
    }
}
disabledetail = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "disabledetail")) {
        bindUpdateStatus("停用", "2");
    }
}

bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "收费项目明细")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../ItemDetail/UpdateItemDetailStatus", rows[0].ItemDetailID, "1", "#grid");
    }
}
copy = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "copy")) {
        Easy.OpenDialogEvent("#copyItem", "复制收费项", 400, 400, "../Item/ItemCopy", "#copyItem-buttons");
    }
};
copydetail = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "copydetail")) {
        Easy.OpenDialogEvent("#copyItemDetail", "复制收费项明细", 400, 400, "../Item/ItemCopyDetail", "#copyItemDetail-buttons");
    }
};

initItemDetail = function () {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        showFooter: true,
        columns: [[
            { field: "ItemDetailID", hidden: true },
            { field: "Detail", title: "收费项", width: 100, sortable: true },
            { field: "Sort", title: "分类", width: 70, sortable: true },
            { field: "IsGive", title: "类别", width: 40, sortable: true },
            { field: "Money", title: "金额", sortable: true, halign: 'left', align: 'right' },
            { field: 'IsReport', title: '报表列', sortable: true, width: 50 },
            { field: 'IsShow', title: '显示列', sortable: true, width: 50 },
            { field: "Queue", title: "排序", sortable: true, halign: 'left', align: 'right' },
            { field: "Status", title: "状态", width: 50, sortable: true }
        ]],
        toolbar: Easy.loadToolbar(menuId, "2"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}


initProfessionGrid = function (itemId) {
    $("#gridProfession").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        pagePosition: "bottom",
        queryParams: { ItemID: itemId },
        columns: [[
            { field: "sItemsProfessionID", checkbox: true },
            { field: "Name", title: "专业名称", width: 200, sortable: true }
        ]],
        toolbar: Easy.loadToolbar(menuId, "4"), sortName: "sItemsProfessionID", sortOrder: "asc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
};

initGiveGrid = function (itemId) {
    $("#gridGive").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: { ItemID: itemId },
        columns: [[
            { field: "sItemsGiveID", checkbox: true },
            { field: "Name", title: "配品名称", width: 200, sortable: true },
            { field: "Queue", title: "排序", width: 70, sortable: true }
        ]],
        toolbar: Easy.loadToolbar(menuId, "5"), sortName: "sItemsGiveID", sortOrder: "asc",
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
};



addpro = function () {
    if (Easy.bindPowerValidationEvent(menuId, "4", "addpro")) {
        var nodes = $("#itemTree").treegrid("getSelected");
        if (Easy.checkNode(nodes, "招生方案")) {
            if (nodes.IsPlan != "是") {
                Easy.centerShow("系统消息", "您选择的不是招生方案，请重新选择！", 1000);
                return false;
            }
            ItemID = nodes.id;
            Easy.OpenDialogEvent("#editItemPro", "编辑招生方案专业设置", 500, 500, "../sItemsProfession/sItemsProfessionEdit", "#editItemPro-buttons");
        }
    }
};

deletepro = function () {
    if (Easy.bindPowerValidationEvent(menuId, "4", "addpro")) {
        var rows = $("#gridProfession").datagrid("getSelections");
        if (rows.length === 0) {
            Easy.centerShow("系统消息", "请选择专业！", 2000);
            return false;
        }
        var idString = "";
        for (var i = 0; i < rows.length; i++) {
            idString += rows[i].sItemsProfessionID + ",";
        }
        idString = idString.substring(0, idString.length - 1);
        Easy.bindUpdateValue("删除", "", "../sItemsProfession/GetDetelesItemProfession", idString, "1", "#gridProfession");

    }
};
addgive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "5", "addgive")) {
        var nodes = $("#itemTree").treegrid("getSelected");
        if (Easy.checkNode(nodes, "招生方案")) {
            if (nodes.IsPlan != "是") {
                Easy.centerShow("系统消息", "您选择的不是招生方案，请重新选择！", 1000);
                return false;
            }
            ItemID = nodes.id;
            Easy.OpenDialogEvent("#editItemGive", "编辑招生方案配品设置", 400, 400, "../sItemsGive/sItemsGiveEdit", "#editItemGive-buttons");
        }
    }
};
deletegive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "5", "deletegive")) {
        var rows = $("#gridGive").datagrid("getSelections");
        if (rows.length === 0) {
            Easy.centerShow("系统消息", "请选择专业！", 2000);
            return false;
        }
        var idString = "";
        for (var i = 0; i < rows.length; i++) {
            idString += rows[i].sItemsGiveID + ",";
        }
        idString = idString.substring(0, idString.length - 1);
        Easy.bindUpdateValue("删除", "", "../sItemsGive/GetDeletesItemsGive", idString, "1", "#gridGive");

    }
};

loadItemDetailData = function (itemId) {
    $("#grid").datagrid({
        queryParams: { ItemID: itemId },
        url: "../ItemDetail/GetItemDetailList", sortName: "Queue", sortOrder: "asc",
    });
};

loadProfessionData = function (itemId) {
    $("#gridProfession").datagrid({
        queryParams: { ItemID: itemId },
        url: "../sItemsProfession/GetItemsProfessionList"
    });
}

loadGiveData = function (itemId) {
    $("#gridGive").datagrid({
        queryParams: { ItemID: itemId },
        url: "../sItemsGive/GetsItemsGiveList"
    });
}
bindSaveandContinue = function () {
    $("#btnCopyAndContinue").click(function () {
        if ($("#btnCopyAndContinue").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            if (Easy.bindPowerValidationEvent(menuId, "1", "copy")) {
                $("#fCopy").form("submit", {
                    url: "../Item/GetItemCopy",
                    onSubmit: function () {
                        var validate = $("#fCopy").form("validate");//验证
                        if (validate) {
                            $("#btnCopyAndContinue").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (result) {
                        if (result === "yes") {
                            $("#ToDeptID").combotree("setValue", "");
                            $("#ToItemID").combotree("loadData", "");
                            $("#ToItemID").combotree("clear");
                            Easy.centerShow("系统消息", "保存成功", 3000);
                        }
                        else {
                            Easy.centerShow("系统消息", result, 3000);
                        }
                        $("#btnCopyAndContinue").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
};

bindSaveItem = function () {
    $("#btnSave").click(function () {
        if ($("#btnSave").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            var buttonCode = "";
            if ($("#ItemID").val() === "") {
                buttonCode = "add";
            }
            else {
                buttonCode = "edit";
            }
            if (Easy.bindPowerValidationEvent(menuId, "1", buttonCode)) {
                $("#fSave").form("submit", {
                    url: "../Item/GetItemEdit",
                    onSubmit: function () {
                        var validate = $("#fSave").form("validate");//验证
                        if (validate) {
                            $("#btnSave").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.Message === "success") {
                            $('#itemTree').treegrid("reload");//刷新树
                            setTimeout(function () {
                                $('#itemTree').treegrid("expandTo", result.Data);
                                var node = $('#itemTree').treegrid("select", result.Data);
                                $('#itemTree').tree("scrollTo", node.target);
                            }, 100);
                            Easy.centerShow("系统消息", "保存成功", 3000);
                            $("#editItem").dialog('close');
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                        $("#btnSave").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
};

tabsSelect = function (i) {
    var nodes = $("#itemTree").treegrid("getSelected");
    if (i === 0)
        loadItemDetailData(nodes.id);
    else if (i === 1)
        loadProfessionData(nodes.id);
    else if (i === 2)
        loadGiveData(nodes.id);
    else if (i === 3)
        initDiscountPlan(nodes.id, "../DiscountPlan/GetDiscountPlanList");
  
    $('#mainTabs').tabs({
        border: false,
        selected: parseInt(i),
        onSelect: function (title, index) {
            if (Easy.checkNode(nodes, "收费项")) {
                if (index === 0) {
                    loadItemDetailData(nodes.id);
                }
                else if (index === 1) {
                    loadProfessionData(nodes.id);
                }
                else if (index === 2) {
                    loadGiveData(nodes.id);
                }
                else if (index === 3) {
                    initDiscountPlan(nodes.id, "../DiscountPlan/GetDiscountPlanList");
                }
            }
        }
    });
};
//加载树
initTree = function (deptId) {
    $('#itemTree').treegrid({
        url: "../Item/GetItemTree",
        idField: 'id',
        treeField: 'text',
        striped: true,
        singleSelect: true,
        queryParams: { Status: "1", DeptID: deptId },
        columns: [[
            { field: 'Month', hidden: true },
            { field: 'text', title: '项目名称', width: 320 },
            { field: 'Sort', title: '类别', width: 40 },
            { field: 'EnglishName', title: '首字母', width: 50 },
            { field: 'IsPlan', title: '方案', width: 40 },
            { field: 'PlanLevel', title: '学习层次', sortable: true, width: 50 },
            { field: 'IsReport', title: '报表列', width: 40 },
            { field: 'IsShow', title: '显示列', width: 40 },
            { field: 'YearName', title: '年份', width: 50 },
            { field: 'MonthName', title: '月份', width: 50 },
            { field: 'StartTime', title: '开始时间', width: 80 },
            { field: 'EndTime', title: '截止时间', width: 80 },
            { field: 'LimitTime', title: '缴费截止时间', width: 90 },
            { field: 'Queue', title: '排序', width: 40 }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1"),
        onSelect: function (rowData) {
            if (Easy.bindPowerValidationEvent(menuId, "2", "viewdetail")) {
                loadItemDetailData(rowData.id);
                ItemID = rowData.id;
                if (rowData.IsPlan === "是") {
                    tabsSelect(1);
                }
                else {
                    tabsSelect(0);
                }
            }
        },
        onLoadSuccess: function (row, data) {
            if (data.length > 0) {
                $(this).treegrid('collapseAll');
                var rootNode = $(this).treegrid('getRoot');
                $(this).treegrid('expand', rootNode.id);
            }
        }, onContextMenu: function (e, node) {
            if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
                e.preventDefault();
                $(this).treegrid('select', node.id);
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        }
    });
}


bindSaveItemDetailandContinue = function () {
    $("#btnCopyDetailAndContinue").click(function () {

        if ($("#btnCopyDetailAndContinue").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            if (Easy.bindPowerValidationEvent(menuId, "2", "copydetail")) {

                $("#fCopys").form("submit", {
                    url: "../Item/GetCopyDetail",
                    onSubmit: function () {
                        var validate = $("#fCopys").form("validate");//验证
                        if (validate) {
                            $("#btnCopyDetailAndContinue").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (result) {
                        if (result === "yes") {
                            $("#toDeptId").combotree("setValue", "");
                            $("#toItemId").combotree("loadData", "");
                            $("#toItemId").combotree("setValue", "");
                            Easy.centerShow("系统消息", "保存成功", 2000);
                        }
                        else {
                            Easy.centerShow("系统消息", result, 2000);
                        }
                        $("#btnCopyDetailAndContinue").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
};

make = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "check")) {
        $.messager.progress({ title: "提示", text: "正在生成,请稍后..." });
        var result = Easy.bindSelectInfo("../Item/MakeData", "");
        if (result.IsError === false) {
            $.messager.progress('close');
            Easy.centerShow("系统消息", "收费项生成成功", 3000);
        }
    }
}

check = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "check")) {
        $.messager.confirm('提示', '确定开始校验？', function (yes) {
            if (yes) {
                var result = Easy.bindSelectInfo("../Item/DataContrast", "");
                if (result.IsError === false) {
                    if (result.Message != "") {
                        itemData = result.Data;
                        detailData = result.Message;
                    }
                    $.messager.progress('close');
                    Easy.OpenDialogEvent("#check", "验证结果", 800, 600, "../Item/ItemCheck", "#check-buttons");
                }
            }
        });




    }
}

change = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "change")) {
        $("#grid").datagrid({
            queryParams: { ItemID: ItemID, Is: 'Yes' },
            url: "../ItemDetail/GetItemDetailList", sortName: "Queue", sortOrder: "asc",
        });
    }
}

derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        if (itemId.length === 0) {
            Easy.centerShow("系统消息", "请选择要导出的方案", 3000);
            return false;
        }
        window.location.href = "../Item/DownLoadItem?itemId=" + itemId + "";
    }
}
itemReload = function () {
    $('#itemTree').treegrid('reload');
}
//添加
addDownload = function () {
    var nodes = $("#itemTree").treegrid("getSelected");
    if (nodes === null) {
        Easy.centerShow("系统消息", "请选择要导出的方案", 3000);
        return false;

    }
    var chuildren = $("#itemTree").treegrid("getChildren", nodes.id);
    var flag = false;
    for (var i = 0; i < chuildren.length; i++) {
        if (chuildren[i].IsPlan === "是") {
            flag = true;
            break;
        }
        else {
            flag = false;
        }
    }

    if (nodes.IsPlan != "是" && !flag) {
        Easy.centerShow("系统消息", "您选择的节点没有方案", 3000);
        return false;
    }

    flag = false;
    var planName = "";
    if (nodes.IsPlan === "是") {
        for (var i = 0; i < itemId.length; i++) {
            if (nodes.id === itemId[i]) {
                flag = true;
                planName = nodes.text;
                break;
            }
        }
    }
    else {
        for (var i = 0; i < chuildren.length; i++) {
            var istrue = false;
            for (var j = 0; j < itemId.length; j++) {
                if (chuildren[i].id === itemId[j]) {
                    flag = true;
                    istrue = true;
                    planName = chuildren[i].text;
                    break;
                }
                else {
                    flag = false;
                }
            }
            if (istrue) {
                break;
            }

        }
    }

    if (flag) {
        Easy.centerShow("系统消息", "【" + planName + "】 已经加入!", 3000);
        return false;
    }
    if (nodes.IsPlan === "是") {
        itemId.push(nodes.id);
    }
    else {
        for (var i = 0; i < chuildren.length; i++) {
            if (chuildren[i].IsPlan === "是") {
                itemId.push(chuildren[i].id);
            }
        }
    }
    Easy.centerShow("系统消息", "加入成功！", 1000);
};
//设置为空
setNull = function () {
    itemId = [];
    Easy.centerShow("系统消息", "操作成功！", 1000);
};
//上传
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        var nodes = $("#itemTree").treegrid("getSelected");
        Easy.OpenDialogEvent("#upItem", "导入方案", 400, 400, "../Item/ItemUpload", "#upItem-buttons");
    }
}
//保存上传方案
bindSaveItemUploadMehod = function () {
    $("#btnUpItem").click(function () {
        if ($("#btnUpItem").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
                $("#fupload").form("submit", {
                    url: "../Item/UploadItem",
                    onSubmit: function () {
                        var validate = $("#fupload").form("validate");//验证
                        if (validate) {
                            $("#btnUpItem").linkbutton("disable");//禁用按钮
                            $.messager.progress({ title: "提示", text: "保存中,请稍后..." });
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.IsError === false) {
                            $("#upItem").dialog('close');
                            Easy.centerShow("系统消息", "操作成功！", 2000);
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 1000);
                        }
                        $.messager.progress('close');
                        $("#btnUpItem").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    })
}
initDiscountPlan = function (itemId, url) {
    $("#gridDiscount").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        url: url,
        queryParams: { ItemID: itemId },
        columns: [[
            { field: "DiscountPlanID", checkbox: true },
            { field: "Name", title: "优惠方案名称", width: 200, sortable: true },
            { field: "Money", title: "优惠总金额", width: 70, sortable: true },
            { field: "Status", title: "状态", width: 70, sortable: true }
        ]],
        toolbar: Easy.loadToolbar(menuId, "3"), sortName: "DiscountPlanID", sortOrder: "asc",
        rowStyler: function (index, row) {
            if (row.Status === "停用") {
                return "color:#ff0000;";
            }
        }
    });
};



addDiscountPlan = function () {
    if (Easy.bindPowerValidationEvent(menuId, "3", "addDiscountPlan")) {
        DiscountPlanID = "0";
        var nodes = $("#itemTree").treegrid("getSelected");
        if (Easy.checkNode(nodes, "招生方案")) {
            if (nodes.IsPlan != "是") {
                Easy.centerShow("系统消息", "您选择的不是招生方案，请重新选择！", 1000);
                return false;
            }
            ItemID = nodes.id;
            Easy.OpenDialogEvent("#editDiscountPlan", "优惠方案设置", 800, 600, "../DiscountPlan/DiscountPlanEdit", "#editDiscountPlan-buttons");
        }
    }
};

bindSaveAddDiscountPlan = function () {
    $("#btnSaveDiscountPlan").click(function () {
        var planName = $("#DiscountName").textbox("getValue");
        var discountPlanId = $("#DiscountPlanID").textbox("getValue");
        if (planName === "") {
            Easy.centerShow("系统消息", "请填写优惠方案名称！", 2000);
            return false;
        }
        var data = $("#gridDiscountDetail").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "请添加优惠明细！", 2000);
            return false;
        }
        var result = Easy.bindSelectInfomation("../DiscountPlan/GetDiscountPlanEdit", { PlanName: planName, Detail: JSON.stringify(data.rows), DiscountPlanID: discountPlanId, ItemID: ItemID });
        if (result.IsError===false) {
            Easy.centerShow("系统消息", result.Message, 2000);
            $("#gridDiscount").datagrid("reload");
            $("#editDiscountPlan").dialog("close");
        }
        else {
            Easy.centerShow("系统消息", result.Message, 2000);
        }
    });
};
editDiscountPlan = function () {
    if (Easy.bindPowerValidationEvent(menuId, "3", "editDiscountPlan")) {
        var rows = $("#gridDiscount").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "优惠方案")) {
            DiscountPlanID = rows[0].DiscountPlanID;
            Easy.OpenDialogEvent("#editDiscountPlan", "优惠方案设置", 800, 600, "../DiscountPlan/DiscountPlanEdit", "#editDiscountPlan-buttons");
        }
       
    }
};
enableDiscountPlan = function () {
    if (Easy.bindPowerValidationEvent(menuId, "3", "enableDiscountPlan")) {
        bindUpdateDiscountPlanStatus("启用","1");
    }
};
disableDiscountPlan = function () {
    if (Easy.bindPowerValidationEvent(menuId, "3", "disableDiscountPlan")) {
        bindUpdateDiscountPlanStatus("停用", "2");
    }
};

bindUpdateDiscountPlanStatus = function (confirmstr, status) {
    var rows = $("#gridDiscount").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "优惠方案")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../DiscountPlan/Disable", rows[0].DiscountPlanID, "1", "#gridDiscount");
    }
}
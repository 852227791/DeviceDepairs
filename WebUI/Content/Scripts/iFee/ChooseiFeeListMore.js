//绑定费用项表单数据
initItemDetailGrid = function () {
    $("#itemDetailGrid").edatagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        columns: [[
            { field: "ItemDetailID", hidden: true },
            { field: "IsFixed", hidden: true },
            { field: "ItemDetailName", title: "收费项目", width: 100 },
            {
                field: "FeeMode", title: "缴费方式", width: 80, editor: { type: 'combobox', options: { url: "../Refe/SelList?RefeTypeID=6", valueField: "Value", textField: "RefeName", panelHeight: 120, editable: false } },
                formatter: function (value, row, index) {
                    return row.FeeModeText;
                }
            },
            { field: "ShouldMoney", title: "应收金额", width: 80, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "PaidMoney", title: "实收金额", width: 80, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "DiscountMoney", title: "优惠金额", width: 80, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "OffSetMoney", title: "充抵金额", width: 80 },
            {
                field: " ", title: "操作", width: 40, formatter: function (value, row, index) {
                    return '<a href="javascript:void(0);" onclick="OffSet()">充抵</a>';
                }
            },
            { field: "Explain", title: "打印说明", width: 80, editor: { type: 'text' } },
            { field: "Remark", title: "系统备注", width: 80, editor: { type: 'text' } },
            { field: "OffsetData", hidden: true }
        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '添加',
            handler: AddItem
        }, '-', {
            iconCls: 'icon-remove',
            text: '移除',
            handler: RemoveItem
        }],
        onBeforeEdit: function (rowIndex, rowData) {
            if (rowData.IsFixed == "1") {
                $("#itemDetailGrid").datagrid('options').columns[0][4].editor.options = { precision: 2, min: 0, readonly: true };
            }
            else {
                $("#itemDetailGrid").datagrid('options').columns[0][4].editor.options = { precision: 2, min: 0 };
            }
        },
        onBeforeSave: function (index, row) {
            var ed = $("#itemDetailGrid").datagrid('getEditor', { index: index, field: 'FeeMode' });
            var refename = $(ed.target).combobox('getText');
            $("#itemDetailGrid").datagrid('getRows')[index]['FeeModeText'] = refename;
        }
    });
    var tempData = $("#AddChooseItemDetail").textbox("getValue");
    if (tempData != null && tempData != "") {
        $("#itemDetailGrid").datagrid("loadData", JSON.parse(tempData));
    }
}

//添加事件
AddItem = function () {
    var row = $("#itemDetailTree").tree("getSelected");//获取选中行
    if (row != null) {
        if (!$("#itemDetailTree").tree('isLeaf', row.target)) {
            Easy.centerShow("系统消息", "请选择末级节点", 1000);
        }
        else {
            var data = $('#itemDetailGrid').datagrid("getData");
            //for (var i = 0; i < data.rows.length; i++) {
            //    if (row.id == data.rows[i]["ItemDetailID"]) {
            //        Easy.centerShow("系统消息", "同一收费项不能添加两次", 1000);
            //        return false;
            //    }
            //}
            var obj = getItemDetail(row.id);
            if (obj == undefined || obj == null) {
                Easy.centerShow("系统消息", "未设置收费明细", 1000);
            }
            else {
                var should = 0;
                if (obj.Sort == "1") {
                    //固定金额
                    should = obj.Money;
                }
                $('#itemDetailGrid').datagrid('appendRow', {
                    ItemDetailID: row.id,
                    IsFixed: obj.Sort,
                    ItemDetailName: row.text,
                    FeeMode: '',
                    FeeModeText: '',
                    ShouldMoney: should,
                    PaidMoney: 0,
                    DiscountMoney: 0,
                    OffSetMoney: 0,
                    Explain: '',
                    Remark: '',
                    OffsetData: '[]'
                });
            }
        }
    }
    else {
        Easy.centerShow("系统消息", "请选择项目信息", 1000);
    }
}

//移除事件
RemoveItem = function () {
    var row = $('#itemDetailGrid').datagrid("getSelected");
    if (row != null) {
        var index2 = $('#itemDetailGrid').datagrid('getRowIndex', row);
        $('#itemDetailGrid').datagrid('deleteRow', index2);
    } else {
        Easy.centerShow("系统消息", "请选择收费项目", 1000);
    }
}

loadItemTree = function (deptId) {
    $("#itemDetailTree").tree({
        url: "../ItemDetail/ItemDetailAllCombotree",
        queryParams: { DeptID: deptId, Type: "2" },
        animate: true,
        lines: true,
        onDblClick: function (node) {
            AddItem();
        }
    });
}

getItemDetail = function (itemDetailId) {
    itemDetailId = itemDetailId.substring(itemDetailId.indexOf("_") + 1, itemDetailId.length);
    var result = Easy.bindSelectInfo("../ItemDetail/GetMoneyAll", itemDetailId.toString());
    var p = JSON.parse(result.Message)[0];
    return p;
}

OffSet = function () {
    $("#itemDetailGrid").edatagrid('saveRow');
    Easy.OpenDialogEvent("#addchooseOffset", "选择充抵项目", 800, 600, "../iFee/ChooseOffsetMore", "#addchooseOffset-buttons");
}

$("#btnAddSaveOffset").click(function () {
    var offsetMoney = 0;
    var data = $('#chooseoffsetgrid2').datagrid("getData");
    var index = $("#itemDetailGrid").datagrid('getRowIndex', $("#itemDetailGrid").datagrid("getSelected"));
    if (data.rows.length > 0) {
        for (var i = 0; i < data.rows.length; i++) {
            offsetMoney += parseFloat(data.rows[i].OffSetMoney);
        }
    }
    $("#itemDetailGrid").datagrid('updateRow', {
        index: index,
        row: {
            OffsetData: JSON.stringify(data),
            OffSetMoney: offsetMoney.toFixed("2")
        }
    });
    $("#addchooseOffset").dialog('close');
});

SearchItemDetailEvent = function () {
    $("#SItemDetail").textbox({
        width: 100
    });
}

btnSearchEvent = function () {
    $("#btnSSearch").click(function () {
        queryHandler($("#SItemDetail").textbox("getValue"));
    });
}

function queryHandler(searchText) {
    $('#itemDetailTree').tree("search", searchText);
}

loadItemTree(DeptID);

initItemDetailGrid();

SearchItemDetailEvent();

btnSearchEvent();

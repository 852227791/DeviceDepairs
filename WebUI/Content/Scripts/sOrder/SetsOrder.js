//绑定费用项表单数据
initItemDetailGrid = function () {
    $("#itemDetailGrid").edatagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        columns: [[
            { field: "DetailID", hidden: true },
            { field: "DetailName", title: "收费项目", width: 90 },
            {
                field: "Sort", title: "项目分类", width: 100, editor: { type: 'combobox', options: { url: "../Refe/SelList?RefeTypeID=5", valueField: "Value", textField: "RefeName", panelHeight: 120, editable: false } },
                formatter: function (value, row, index) {
                    return row.SortName;
                }
            },
            {
                field: "IsGive", title: "类别", width: 60, editor: { type: 'combobox', options: { url: "../Refe/SelList?RefeTypeID=15", valueField: "Value", textField: "RefeName", panelHeight: 120, editable: false } },
                formatter: function (value, row, index) {
                    return row.IsGiveName;
                }
            },
            { field: "ShouldMoney", title: "应收金额", width: 80, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "LimitTime", title: "缴费截止时间", width: 100, editor: { type: 'datebox', options: { editable: false } } }
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
        onBeforeSave: function (index, row) {
            var ed = $("#itemDetailGrid").datagrid('getEditor', { index: index, field: 'Sort' });
            var refename = $(ed.target).combobox('getText');
            $("#itemDetailGrid").datagrid('getRows')[index]['SortName'] = refename;

            var ed2 = $("#itemDetailGrid").datagrid('getEditor', { index: index, field: 'IsGive' });
            var refename2 = $(ed2.target).combobox('getText');
            $("#itemDetailGrid").datagrid('getRows')[index]['IsGiveName'] = refename2;
        }
    });
    var tempData = $("#sOrderContent").textbox("getValue");
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
            for (var i = 0; i < data.rows.length; i++) {
                if (row.id == data.rows[i]["DetailID"]) {
                    Easy.centerShow("系统消息", "同一收费项不能添加两次", 1000);
                    return false;
                }
            }
            $('#itemDetailGrid').datagrid('appendRow', {
                DetailID: row.id,
                DetailName: row.text,
                Sort: '',
                SortName: '',
                IsGive: '',
                IsGiveName: '',
                ShouldMoney: 0,
                LimitTime: ''
            });
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

loadItemTree = function () {
    $("#itemDetailTree").tree({
        url: "../Detail/GetDetailCommonbox",
        animate: true,
        lines: true,
        onDblClick: function (node) {
            AddItem();
        }
    });
}

loadItemTree();

initItemDetailGrid();

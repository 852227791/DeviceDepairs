if (selectRow!=null) {
    $("#fSave").form('load',selectRow)

}
queryHandler = function (searchText, event) {
    $('#ParentID').combotree('tree').tree("search", searchText);
}
$("#ParentID").combotree({
    valueField: 'id',
    textField: 'text',
    method: 'get',
    url: '../Content/Json/area.json',
    panelHeight: 300,
    panelWidth: 300,
    keyHandler: {
        query: queryHandler
    },
    onHidePanel: function (node) {
        var nodeTree = $('#ParentID').combotree('tree').tree("getSelected");
        if (nodeTree != null) {
            $("#ParentID").combotree("setValue", nodeTree.id);
        }
    }

});

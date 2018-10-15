var zTreeOperation = {};

(function (me) {

    //查看项的
    me.changeNode = function (data, zTreeObj) {
        var nodes = zTreeObj.getNodes();
        //数据转换为简单 Array 格式
        var childNodes = zTreeObj.transformToArray(nodes);


        //循环树的元素
        childNodes.forEach(function (item, index, arr) {

            var id = item.id;
            var name = item.name;
            var index = name.indexOf("(");

            var iscontain = array_contain(data, id);

            //判断是否包含元素
            if (iscontain.state) {
                item.name = index != -1 ? name.substring(0, index) + "(" + iscontain.value + ")" : name + "(" + iscontain.value + ")";
                zTreeObj.updateNode(item);
            } else {
                //清理原来的数据
                if (index != -1) {
                    item.name = name.substring(0, index);
                    zTreeObj.updateNode(item);
                }
            }

        });
    };

    //类型 数组是否包含元素
    function array_contain(array, obj) {

        if (array != null && array != undefined) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].Code == obj)
                    return { state: true, value: array[i].Num };
            }
        }
        return { state: false, value: 0 };
    }

})(zTreeOperation)
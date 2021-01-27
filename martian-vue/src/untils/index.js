
// 在table的数据显示可以用     格式 parseTime(date, "{y}-{m}-{d}")
export function parseTime(time, cFormat) {
  if (arguments.length === 0) {
    return null;
  }
  const format = cFormat || "{y}-{m}-{d} {h}:{i}:{s}";
  let date;
  if (typeof time === "object") {
    date = time;
  } else {
    if (("" + time).length === 10) time = parseInt(time) * 1000;
    date = new Date(time);
  }
  const formatObj = {
    y: date.getFullYear(),
    m: date.getMonth() + 1,
    d: date.getDate(),
    h: date.getHours(),
    i: date.getMinutes(),
    s: date.getSeconds(),
    a: date.getDay()
  };
  const time_str = format.replace(/{(y|m|d|h|i|s|a)+}/g, (result, key) => {
    let value = formatObj[key];
    if (key === "a")
      return ["一", "二", "三", "四", "五", "六", "日"][value - 1];
    if (result.length > 0 && value < 10) {
      value = "0" + value;
    }
    return value || 0;
  });
  return time_str;
}

// 在创建编号等可以用     格式 formatTime("yyyyMMddhhmmss")
export function formatTime(format) {
  const now = new Date();
  var o = {
    "M+": now.getMonth() + 1, // month
    "d+": now.getDate(), // day
    "h+": now.getHours(), // hour
    "m+": now.getMinutes(), // minute
    "s+": now.getSeconds(), // second
    "q+": Math.floor((now.getMonth() + 3) / 3), // quarter
    S: now.getMilliseconds() // millisecond
  };
  if (/(y+)/.test(format)) {
    format = format.replace(
      RegExp.$1,
      (now.getFullYear() + "").substr(4 - RegExp.$1.length)
    );
  }
  for (var k in o) {
    if (new RegExp("(" + k + ")").test(format)) {
      format = format.replace(
        RegExp.$1,
        RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length)
      );
    }
  }
  return format;
}


// 深拷贝
export const deepClone = obj => {
  if (!isObject(obj)) {
    throw new Error("obj 不是一个对象！");
  }

  const isArray = Array.isArray(obj);
  const cloneObj = isArray ? [] : {};
  for (const key in obj) {
    cloneObj[key] = isObject(obj[key]) ? deepClone(obj[key]) : obj[key];
  }

  return cloneObj;
};
const isObject = o => {
  return (typeof o === "object" || typeof o === "function") && o !== null;
};


export function downloadAs(blob, fileName) {
  var downloadElement = document.createElement("a");
  var href = window.URL.createObjectURL(blob);
  downloadElement.href = href;
  downloadElement.download = fileName;
  document.body.appendChild(downloadElement);
  downloadElement.click();
  document.body.removeChild(downloadElement);
  window.URL.revokeObjectURL(href);
}

// 清空页面缓存 调用 clearPageCache(this.$vnode);
export function clearPageCache(node) {
  var key =
    node.key == null
      ? node.componentOptions.Ctor.cid +
      (node.componentOptions.tag
        ? `::${node.componentOptions.tag}`
        : "")
      : node.key;
  var cache = node.parent.componentInstance.cache;
  var keys = node.parent.componentInstance.keys;
  if (cache[key]) {
    if (keys.length) {
      var index = keys.indexOf(key);
      if (index > -1) {
        keys.splice(index, 1);
      }
    }
    delete cache[key];
  }
}
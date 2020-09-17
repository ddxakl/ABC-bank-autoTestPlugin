import {ABCAutoTest} from "./ABCTest";
import PointBury from './burying-point.js';
export default {
  install(Vue, pluginOption = {}) {
    //根据vue页面定义的name获取vue页面实例对象
    window.findVueByName = function(name) {
      return getVuePageByName(name);
    };
    //用于控制录制的开始和结束
    document.onkeydown = function(event) {
      var e = event || window.event;
      var ctrlKey = e.ctrlKey || e.metaKey;
      var shiftKey = e.shiftKey || e.metaKey;
      var keyCode = e.keyCode || e.which || e.charCode;
      if (ctrlKey && shiftKey && keyCode === 82) {
        window.flag = true;
        //单次录制记录
        ABCAutoTest.StartRecordTrade();
        window.controlMap = {};
        //录制完成之后在记录
        //window.clearRecContext();
        alert("录制开始");
      } else if (ctrlKey && shiftKey && keyCode === 81) {
        //PointBury.SaveBuryingPointData();
        //单次录制
        window.flag = false;
        ABCAutoTest.EndRecordTrade();
        //录制完成之后再记录
        //ABCAutoTest.RecordTrade();
        alert("录制结束");
      }else if (ctrlKey && shiftKey && keyCode === 65) {
        //结束数据埋点，将信息存储到本地
        PointBury.SaveBuryingPointData();
      }
    };
    //控制是否记录录制数据
    window.flag = false;
    //用于存放录制的组件信息（随时录制随时记录）
    window.controlMap = {};
    //用于存放录制的数据（录制完成之后再记录）
    window.recContext = [];
    //获取录制的数据
    window.getRecContext = function() {
      var jsonstr = JSON.stringify(window.recContext);
      console.log("录制的信息" + jsonstr);
      return jsonstr;
    };
    //清空录制的数据
    window.clearRecContext = function() {
      window.recContext=[];
      //window.recContext.splice(0, window.recContext.length);
    };
    //组件聚焦方法
    window.focusEvent = function(name, ref) {
      let element;
      element = getNativeElement(name, ref, element);
      if (element) {
        element.focus();
      }
    };
    //组件失焦方法
    window.blurEvent = function(name, ref) {
      let element;
      element = getNativeElement(name, ref, element);
      if (element) {
        element.blur();
      }
    };
  }
};

//根据vue页面定义的name获取vue页面实例对象
function getVuePageByName(name) {
  let node = getVuePageById(name);
  let dialog = getDialogPage(name);
  if (node) {
    return node;
  } else if(dialog){
    return dialog;
  } else {
    return getVuePageByClassName(name);
  }
}

//查找id=app下vue页面
function getVuePageById(name) {
  let node = document.getElementById('app').__vue__;
  let optionName = node.$options.name;
  if (optionName == name) {
    return node;
  } else {
    return traversalVuePage(node, name);
  }
}

//查找ClassName=dialog-window下vue页面
//查找弹出框的页面
let dialogList = ['dialog-window','dialog-news'];
function getVuePageByClassName(name) {
  let elements = document.getElementsByClassName('dialog-window');
  for (let i = 0; i < elements.length; i++) {
    console.log(elements[i]);
    let nodes = elements[i].children; 
    for (let k = 0; k < nodes.length; k++) {
      let node = nodes[i].__vue__;
      if (node) {
        let optionName = node.$options.name;
        if (optionName == name) {
          return node;
        } else {
          return traversalVuePage(node, name);
        }
      }
    }
  }
}

function getDialogPage(name){
  let elements = document.getElementsByClassName('dialog-news');
  for(let i = 0; i < elements.length; i++){
    let node = elements[i].__vue__;
    if (node) {
      let optionName = node.$options.name;
      if (optionName == name) {
        return node;
      } else {
        return traversalVuePage(node, name);
      }
    }
  }
}

//遍历vue页面
function traversalVuePage(node, name) {
  let nodes = node.$children;
  for (let i = 0; i < nodes.length; i++) {
    let optionName = nodes[i].$options.name;
    if (optionName == name) {
      return nodes[i];
    } else {
      let node = traversalVuePage(nodes[i], name);
      if (node != undefined && node != "") {
        return node;
      }
    }
  }
}
//
function getNativeElement(name, ref, element) {
  let page = getVuePageByName(name);
  if (page) {
    let control = page.$refs[ref];
    //control._isVue = true
    if (control._vnode != undefined) {
      element = control.$el;
      if (element.tagName === "INPUT") {
      } else if (element.children) {
        element = traversalElement(element.children);
      }
    } else {
      element = control;
    }
    return element;
  }
}

function traversalElement(childrens) {
  for (let i = 0; i < childrens.length; i++) {
    if (childrens[i].tagName === "INPUT") {
      return childrens[i];
    } else if (childrens[i].children) {
      let element = traversalElement(childrens[i].children);
      if (element != undefined && element != "") {
        return element;
      }
    }
  }
}

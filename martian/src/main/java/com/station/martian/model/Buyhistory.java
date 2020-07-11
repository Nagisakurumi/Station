package com.station.martian.model;

import java.util.Date;

public class Buyhistory {
    private Integer id;

    private Date buytime;

    private Float money;

    private Integer userid;

    private String usernickname;

    private Integer codetype;

    private Date validitydate;

    private Integer activatecodeid;

    private Boolean isused;

    private Date createtime;

    private Integer appendday;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Date getBuytime() {
        return buytime;
    }

    public void setBuytime(Date buytime) {
        this.buytime = buytime;
    }

    public Float getMoney() {
        return money;
    }

    public void setMoney(Float money) {
        this.money = money;
    }

    public Integer getUserid() {
        return userid;
    }

    public void setUserid(Integer userid) {
        this.userid = userid;
    }

    public String getUsernickname() {
        return usernickname;
    }

    public void setUsernickname(String usernickname) {
        this.usernickname = usernickname == null ? null : usernickname.trim();
    }

    public Integer getCodetype() {
        return codetype;
    }

    public void setCodetype(Integer codetype) {
        this.codetype = codetype;
    }

    public Date getValiditydate() {
        return validitydate;
    }

    public void setValiditydate(Date validitydate) {
        this.validitydate = validitydate;
    }

    public Integer getActivatecodeid() {
        return activatecodeid;
    }

    public void setActivatecodeid(Integer activatecodeid) {
        this.activatecodeid = activatecodeid;
    }

    public Boolean getIsused() {
        return isused;
    }

    public void setIsused(Boolean isused) {
        this.isused = isused;
    }

    public Date getCreatetime() {
        return createtime;
    }

    public void setCreatetime(Date createtime) {
        this.createtime = createtime;
    }

    public Integer getAppendday() {
        return appendday;
    }

    public void setAppendday(Integer appendday) {
        this.appendday = appendday;
    }
}
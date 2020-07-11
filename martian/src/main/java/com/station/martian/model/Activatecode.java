package com.station.martian.model;

import java.util.Date;

public class Activatecode {
    private Integer id;

    private String name;

    private Integer codetype;

    private Integer days;

    private Date createtime;

    private Integer byuserid;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name == null ? null : name.trim();
    }

    public Integer getCodetype() {
        return codetype;
    }

    public void setCodetype(Integer codetype) {
        this.codetype = codetype;
    }

    public Integer getDays() {
        return days;
    }

    public void setDays(Integer days) {
        this.days = days;
    }

    public Date getCreatetime() {
        return createtime;
    }

    public void setCreatetime(Date createtime) {
        this.createtime = createtime;
    }

    public Integer getByuserid() {
        return byuserid;
    }

    public void setByuserid(Integer byuserid) {
        this.byuserid = byuserid;
    }
}
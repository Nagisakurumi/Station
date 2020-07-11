package com.station.martian.model;

public class Role {
    private Integer id;

    private String name;

    private String powercount;

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

    public String getPowercount() {
        return powercount;
    }

    public void setPowercount(String powercount) {
        this.powercount = powercount == null ? null : powercount.trim();
    }
}
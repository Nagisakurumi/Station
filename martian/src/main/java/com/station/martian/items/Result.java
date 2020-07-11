package com.station.martian.items;

import lombok.Data;

/**
 * 请求返回
 */
@Data
public class Result {
    /**
     * 返回的请求码
     */
    private int resultCode;
    /**
     * 请求信息
     */
    private String resultMessage;
    /**
     * 返回的数据
     */
    private Object data;

    public Result(){
        resultCode = 0;
        resultMessage = "";
    }
}

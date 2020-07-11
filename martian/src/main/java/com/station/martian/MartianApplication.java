package com.station.martian;

import org.mybatis.spring.annotation.MapperScan;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@MapperScan(value = "com.station.martian.mapper")
@SpringBootApplication
public class MartianApplication {

    public static void main(String[] args) {
        SpringApplication.run(MartianApplication.class, args);
    }

}

package com.station.martian.utils;

import java.io.*;

public class FileUtil {

    /**
     * 保存文件
     * @param path
     * @param stream
     * @return
     */
    public static boolean saveFile(String path, InputStream stream){
        File file = new File(path);
        try(FileOutputStream fileOutputStream = new FileOutputStream(file)){
            BufferedOutputStream writer = new BufferedOutputStream(fileOutputStream);
            writer.write(stream.readAllBytes());
        }catch (Exception e){
            e.printStackTrace();
            return false;
        }finally {

        }
        return true;
    }

    /**
     * 获取本地静态资源路径
     * @return
     */
    public static String getLocalStaticPath(){
        return null;
    }
}

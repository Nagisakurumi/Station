package com.station.martian.mapper;

import com.station.martian.model.Version;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface VersionMapper {
    @Delete({
        "delete from version",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into version (Path, CreateTime, ",
        "News, VersionNum, ",
        "Descript)",
        "values (#{path,jdbcType=VARCHAR}, #{createtime,jdbcType=TIMESTAMP}, ",
        "#{news,jdbcType=VARCHAR}, #{versionnum,jdbcType=VARCHAR}, ",
        "#{descript,jdbcType=VARCHAR})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Version record);

    @Select({
        "select",
        "Id, Path, CreateTime, News, VersionNum, Descript",
        "from version",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Path", property="path", jdbcType=JdbcType.VARCHAR),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="News", property="news", jdbcType=JdbcType.VARCHAR),
        @Result(column="VersionNum", property="versionnum", jdbcType=JdbcType.VARCHAR),
        @Result(column="Descript", property="descript", jdbcType=JdbcType.VARCHAR)
    })
    Version selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, Path, CreateTime, News, VersionNum, Descript",
        "from version"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Path", property="path", jdbcType=JdbcType.VARCHAR),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="News", property="news", jdbcType=JdbcType.VARCHAR),
        @Result(column="VersionNum", property="versionnum", jdbcType=JdbcType.VARCHAR),
        @Result(column="Descript", property="descript", jdbcType=JdbcType.VARCHAR)
    })
    List<Version> selectAll();

    @Update({
        "update version",
        "set Path = #{path,jdbcType=VARCHAR},",
          "CreateTime = #{createtime,jdbcType=TIMESTAMP},",
          "News = #{news,jdbcType=VARCHAR},",
          "VersionNum = #{versionnum,jdbcType=VARCHAR},",
          "Descript = #{descript,jdbcType=VARCHAR}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Version record);
}
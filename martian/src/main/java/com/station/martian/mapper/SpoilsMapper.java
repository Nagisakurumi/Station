package com.station.martian.mapper;

import com.station.martian.model.Spoils;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface SpoilsMapper {
    @Delete({
        "delete from spoils",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into spoils (Name, Type, ",
        "UserId, NickName, ",
        "CreateTime, IsSave, ",
        "Score, Level, Star, ",
        "Content, ImagePath)",
        "values (#{name,jdbcType=VARCHAR}, #{type,jdbcType=INTEGER}, ",
        "#{userid,jdbcType=INTEGER}, #{nickname,jdbcType=VARCHAR}, ",
        "#{createtime,jdbcType=TIMESTAMP}, #{issave,jdbcType=BIT}, ",
        "#{score,jdbcType=INTEGER}, #{level,jdbcType=VARCHAR}, #{star,jdbcType=VARCHAR}, ",
        "#{content,jdbcType=VARCHAR}, #{imagepath,jdbcType=VARCHAR})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Spoils record);

    @Select({
        "select",
        "Id, Name, Type, UserId, NickName, CreateTime, IsSave, Score, Level, Star, Content, ",
        "ImagePath",
        "from spoils",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Name", property="name", jdbcType=JdbcType.VARCHAR),
        @Result(column="Type", property="type", jdbcType=JdbcType.INTEGER),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="NickName", property="nickname", jdbcType=JdbcType.VARCHAR),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="IsSave", property="issave", jdbcType=JdbcType.BIT),
        @Result(column="Score", property="score", jdbcType=JdbcType.INTEGER),
        @Result(column="Level", property="level", jdbcType=JdbcType.VARCHAR),
        @Result(column="Star", property="star", jdbcType=JdbcType.VARCHAR),
        @Result(column="Content", property="content", jdbcType=JdbcType.VARCHAR),
        @Result(column="ImagePath", property="imagepath", jdbcType=JdbcType.VARCHAR)
    })
    Spoils selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, Name, Type, UserId, NickName, CreateTime, IsSave, Score, Level, Star, Content, ",
        "ImagePath",
        "from spoils"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Name", property="name", jdbcType=JdbcType.VARCHAR),
        @Result(column="Type", property="type", jdbcType=JdbcType.INTEGER),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="NickName", property="nickname", jdbcType=JdbcType.VARCHAR),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="IsSave", property="issave", jdbcType=JdbcType.BIT),
        @Result(column="Score", property="score", jdbcType=JdbcType.INTEGER),
        @Result(column="Level", property="level", jdbcType=JdbcType.VARCHAR),
        @Result(column="Star", property="star", jdbcType=JdbcType.VARCHAR),
        @Result(column="Content", property="content", jdbcType=JdbcType.VARCHAR),
        @Result(column="ImagePath", property="imagepath", jdbcType=JdbcType.VARCHAR)
    })
    List<Spoils> selectAll();

    @Update({
        "update spoils",
        "set Name = #{name,jdbcType=VARCHAR},",
          "Type = #{type,jdbcType=INTEGER},",
          "UserId = #{userid,jdbcType=INTEGER},",
          "NickName = #{nickname,jdbcType=VARCHAR},",
          "CreateTime = #{createtime,jdbcType=TIMESTAMP},",
          "IsSave = #{issave,jdbcType=BIT},",
          "Score = #{score,jdbcType=INTEGER},",
          "Level = #{level,jdbcType=VARCHAR},",
          "Star = #{star,jdbcType=VARCHAR},",
          "Content = #{content,jdbcType=VARCHAR},",
          "ImagePath = #{imagepath,jdbcType=VARCHAR}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Spoils record);
}
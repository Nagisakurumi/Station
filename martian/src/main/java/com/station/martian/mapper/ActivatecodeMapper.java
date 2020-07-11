package com.station.martian.mapper;

import com.station.martian.model.Activatecode;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface ActivatecodeMapper {
    @Delete({
        "delete from activatecode",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into activatecode (Name, CodeType, ",
        "Days, CreateTime, ",
        "ByUserId)",
        "values (#{name,jdbcType=VARCHAR}, #{codetype,jdbcType=INTEGER}, ",
        "#{days,jdbcType=INTEGER}, #{createtime,jdbcType=TIMESTAMP}, ",
        "#{byuserid,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Activatecode record);

    @Select({
        "select",
        "Id, Name, CodeType, Days, CreateTime, ByUserId",
        "from activatecode",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Name", property="name", jdbcType=JdbcType.VARCHAR),
        @Result(column="CodeType", property="codetype", jdbcType=JdbcType.INTEGER),
        @Result(column="Days", property="days", jdbcType=JdbcType.INTEGER),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="ByUserId", property="byuserid", jdbcType=JdbcType.INTEGER)
    })
    Activatecode selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, Name, CodeType, Days, CreateTime, ByUserId",
        "from activatecode"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Name", property="name", jdbcType=JdbcType.VARCHAR),
        @Result(column="CodeType", property="codetype", jdbcType=JdbcType.INTEGER),
        @Result(column="Days", property="days", jdbcType=JdbcType.INTEGER),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="ByUserId", property="byuserid", jdbcType=JdbcType.INTEGER)
    })
    List<Activatecode> selectAll();

    @Update({
        "update activatecode",
        "set Name = #{name,jdbcType=VARCHAR},",
          "CodeType = #{codetype,jdbcType=INTEGER},",
          "Days = #{days,jdbcType=INTEGER},",
          "CreateTime = #{createtime,jdbcType=TIMESTAMP},",
          "ByUserId = #{byuserid,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Activatecode record);
}
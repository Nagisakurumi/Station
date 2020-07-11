package com.station.martian.mapper;

import com.station.martian.model.Power;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface PowerMapper {
    @Delete({
        "delete from power",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into power (Name, Type)",
        "values (#{name,jdbcType=VARCHAR}, #{type,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Power record);

    @Select({
        "select",
        "Id, Name, Type",
        "from power",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Name", property="name", jdbcType=JdbcType.VARCHAR),
        @Result(column="Type", property="type", jdbcType=JdbcType.INTEGER)
    })
    Power selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, Name, Type",
        "from power"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Name", property="name", jdbcType=JdbcType.VARCHAR),
        @Result(column="Type", property="type", jdbcType=JdbcType.INTEGER)
    })
    List<Power> selectAll();

    @Update({
        "update power",
        "set Name = #{name,jdbcType=VARCHAR},",
          "Type = #{type,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Power record);
}
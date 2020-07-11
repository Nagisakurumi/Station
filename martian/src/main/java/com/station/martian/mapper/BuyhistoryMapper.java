package com.station.martian.mapper;

import com.station.martian.model.Buyhistory;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface BuyhistoryMapper {
    @Delete({
        "delete from buyhistory",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into buyhistory (BuyTime, Money, ",
        "UserId, UserNickName, ",
        "CodeType, ValidityDate, ",
        "ActivateCodeId, IsUsed, ",
        "CreateTime, AppendDay)",
        "values (#{buytime,jdbcType=TIMESTAMP}, #{money,jdbcType=REAL}, ",
        "#{userid,jdbcType=INTEGER}, #{usernickname,jdbcType=VARCHAR}, ",
        "#{codetype,jdbcType=INTEGER}, #{validitydate,jdbcType=TIMESTAMP}, ",
        "#{activatecodeid,jdbcType=INTEGER}, #{isused,jdbcType=BIT}, ",
        "#{createtime,jdbcType=TIMESTAMP}, #{appendday,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Buyhistory record);

    @Select({
        "select",
        "Id, BuyTime, Money, UserId, UserNickName, CodeType, ValidityDate, ActivateCodeId, ",
        "IsUsed, CreateTime, AppendDay",
        "from buyhistory",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="BuyTime", property="buytime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="Money", property="money", jdbcType=JdbcType.REAL),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="UserNickName", property="usernickname", jdbcType=JdbcType.VARCHAR),
        @Result(column="CodeType", property="codetype", jdbcType=JdbcType.INTEGER),
        @Result(column="ValidityDate", property="validitydate", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="ActivateCodeId", property="activatecodeid", jdbcType=JdbcType.INTEGER),
        @Result(column="IsUsed", property="isused", jdbcType=JdbcType.BIT),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="AppendDay", property="appendday", jdbcType=JdbcType.INTEGER)
    })
    Buyhistory selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, BuyTime, Money, UserId, UserNickName, CodeType, ValidityDate, ActivateCodeId, ",
        "IsUsed, CreateTime, AppendDay",
        "from buyhistory"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="BuyTime", property="buytime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="Money", property="money", jdbcType=JdbcType.REAL),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="UserNickName", property="usernickname", jdbcType=JdbcType.VARCHAR),
        @Result(column="CodeType", property="codetype", jdbcType=JdbcType.INTEGER),
        @Result(column="ValidityDate", property="validitydate", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="ActivateCodeId", property="activatecodeid", jdbcType=JdbcType.INTEGER),
        @Result(column="IsUsed", property="isused", jdbcType=JdbcType.BIT),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="AppendDay", property="appendday", jdbcType=JdbcType.INTEGER)
    })
    List<Buyhistory> selectAll();

    @Update({
        "update buyhistory",
        "set BuyTime = #{buytime,jdbcType=TIMESTAMP},",
          "Money = #{money,jdbcType=REAL},",
          "UserId = #{userid,jdbcType=INTEGER},",
          "UserNickName = #{usernickname,jdbcType=VARCHAR},",
          "CodeType = #{codetype,jdbcType=INTEGER},",
          "ValidityDate = #{validitydate,jdbcType=TIMESTAMP},",
          "ActivateCodeId = #{activatecodeid,jdbcType=INTEGER},",
          "IsUsed = #{isused,jdbcType=BIT},",
          "CreateTime = #{createtime,jdbcType=TIMESTAMP},",
          "AppendDay = #{appendday,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Buyhistory record);
}
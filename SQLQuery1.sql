USE [busData]
GO
/****** Object:  StoredProcedure [dbo].[simulateBus]    Script Date: 04.11.2019 20:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[simulateBus](@busID as nvarchar(20))
AS
BEGIN
DECLARE @StartStation AS INT;
DECLARE @StartTime AS INT;
DECLARE @ActualStation AS INT;
DECLARE @NextStation AS INT;
DECLARE @MinStationNr AS INT;
DECLARE @MaxStationNr AS INT;
DECLARE @TimeTemp AS TIME(0);
DECLARE @FoundStation AS INT;
DECLARE @TimeSum AS TIME(0);
DECLARE @TimeUpper AS TIME(0);
DECLARE @TimeLower AS TIME(0);

DECLARE BusStarts CURSOR
		FOR select temp.stationID, temp.actual_time
			from (
					select stationID ,datediff(minute, startTime, (CONVERT(TIME(0), GETDATE() ))) as actual_time
					from Timetable					--***--- this should be corrected
					where busNr = @busID and weekday = 1) as temp join Line as line on temp.stationID = line.StationID
			where temp.actual_time > 0 and temp.actual_time < 45 and line.LineID = @busID and line.StationNr != (select max(StationNr) from Line where LineID = @busID)
			order by temp.actual_time;

	SET @TimeUpper = '23:00:00.0000000';
	SET @TimeLower = '00:00:00.0000000';

	OPEN BusStarts;
  
	FETCH NEXT FROM BusStarts INTO @StartStation, @StartTime;
	WHILE @@FETCH_STATUS = 0
	BEGIN  
		select @MinStationNr = min(StationNr) from Line where LineID = @busID and StationID = @StartStation;
		select @MaxStationNr = StationNr from Line where LineID = @busID and StationID = (select top (1) StationId from Timetable where busNr = @busID and stationID!=@StartStation);
		
		SET @TimeSum = '00:00:00.0000000';
		SET @TimeSum = dateadd(minute,@StartTime,@TimeSum);
		print '@StartStation';
		print @StartStation;
		print '@StartTime';
		print @TimeSum;
		SET @FoundStation = @StartStation;
		DECLARE List CURSOR
		FOR	select StationID
			from Line
			where LineID = @busID and StationNr >= @MinStationNr and StationNr <= @MaxStationNr;

		OPEN List;
		  
		FETCH NEXT FROM List INTO @ActualStation;
		WHILE @@FETCH_STATUS = 0  
			BEGIN
				FETCH NEXT FROM List INTO @NextStation;
				IF @ActualStation != @NextStation
				Begin
					print '@ActualStation';
					print @ActualStation;
					print '@NextStation';
					print @NextStation;
					select @TimeTemp=duration 
					from [References] as ref
					where ref.[From] = @ActualStation and ref.[To] = @NextStation;
					
					SET @TimeSum = dateadd(minute,(-1)*DATEPART(minute, @TimeTemp),@TimeSum);
					
					
					IF @TimeSum >= @TimeUpper
					BEGIN
						BREAK;
					END;
					IF @TimeLower <= @TimeSum
					BEGIN
						SET @FoundStation = @NextStation;
					END;
					

					print '@@@StartTime';
					print @TimeSum;
					print '------------';
					Set @ActualStation = @NextStation;
				end
			END;

		CLOSE List;
 
		DEALLOCATE List;

		print '@@@FoundStation';
		print @FoundStation;

		UPDATE dbo.SimulatedBus 
		SET LastCheckin = @FoundStation, Measurement_Timestamp = GETDATE()
		WHERE BusId = (@busID+'_'+CONVERT(VARCHAR(10), @StartStation));

		print (@busID+'_'+CONVERT(VARCHAR(10), @StartStation))+' updated!';
		FETCH NEXT FROM BusStarts INTO @StartStation, @StartTime;
	END;

	CLOSE BusStarts;
 
	DEALLOCATE BusStarts;


END;
/*
EXECUTE [simulateBus] 
    @busID = '26'
	/*
	select duration 
	from [References] as ref
	where ref.[From] = 24 and ref.[To] = 25;

select temp.stationID, temp.actual_time
			from (
					select stationID ,datediff(minute, startTime, (CONVERT(TIME(0), GETDATE() ))) as actual_time
					from Timetable					--***--- this should be corrected
					where busNr = 44 and weekday = 1) as temp join Line as line on temp.stationID = line.StationID
			where temp.actual_time > 0 and temp.actual_time < 45 and line.LineID = 44 and line.StationNr != (select max(StationNr) from Line where LineID = 44)
			order by temp.actual_time;

select DATEPART(minute,duration),dateadd(minute,-DATEPART(minute,duration),CONVERT(time(0),('00:1:00.0000000')))
from [References] as ref
where ref.[From] = 24 and ref.[To] = 25;*/*/

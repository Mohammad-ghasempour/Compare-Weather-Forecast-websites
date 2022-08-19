import { ValidationErrors } from 'fluentvalidation-ts/dist/ValidationErrors';
import StatusCodeNotOkError from './apiErrors';
import { weekNo, WeekNoValidator , MyDateValidator , myDate } from './apiTypes';


export const api = (baseUrl: string) => {
    const queryCityByDate = (oneDate: string, cityName: string): string =>
        `${baseUrl}weatherforecast/date?DateQuery.Date=${oneDate}&CityQuery.City=${cityName}`;
    const queryCityByWeekNo = (weekNo: weekNo, cityName: string): string =>
        `${baseUrl}weatherforecast/week?Week=${weekNo}&CityQuery.City=${cityName}`;
    const queryCityBetweenToDates = (
        from: string,
        to: string,
        city: string,
    ): string =>
        `${baseUrl}weatherforecast/between?BetweenDateQuery.From=${from}&BetweenDateQuery.To=${to}&CityQuery.City=${city}`;


    const makeSingleDateApiRequest = async (
        city: string,
        date: string,
    ): Promise<any> => {
        const url = queryCityByDate(date, city);
        const res = await fetch(url);
        if (res.ok) {
            return res.json();
        } else {
            throw new StatusCodeNotOkError(res);
        }
    };
    const makeWeekNumberApiRequest = (cityName: string, weekNo: weekNo) => {
        weekNo;
        console.log(
            '******************************************' +
            weekNo +
            +'value:' +
            weekNo.value,
        );
        const weekNoValidator = new WeekNoValidator();
        const result: ValidationErrors<weekNo> = weekNoValidator.validate(weekNo);
        console.log('validation of weekNo' + result);
        if (result !== {}) {
            //We have an error in weekNi type
            console.log('The object is not valid');
            console.log(result.value);

            console.log(
                result.value === 'week number should be greater or equal to 1',
            );
            console.log(
                result.value === 'week number should be greater or equal to 1',
            );
            return [];
        }

        fetch(queryCityByWeekNo(weekNo, cityName)).then((response) =>
            response.json(),
        );
    };
    //   const makeSingleDateApiRequest = async (
    //     city: string,
    //     date: string,
    //   ): Promise<any> => {
    //     const url = queryCityByDate(date, city);
    //     const res = await fetch(url);
    //     if (res.ok) {
    //       return res.json();
    //     } else {
    //       throw new StatusCodeNotOkError(res);
    //     }
    //   };
    const makeBetweenDatesApiRequest = async (
        cityName: string,
        fromDate: myDate,
        toDate: myDate,
    ): Promise<any> => {
        const url = queryCityBetweenToDates(fromDate, toDate, cityName);
        const dateValidator = new MyDateValidator();
        const result = dateValidator.validate(fromDate)

       
    };
    return {
        makeSingleDateApiRequest,
        makeWeekNumberApiRequest,
        makeBetweenDatesApiRequest,
    };
};

import { RadioButton } from "../../../../../searchBox/RadioButton";
import { Children } from "../../../../compTypes";
import { WeatherForcastEnumType } from "../SelectSearchOptionState";
import { TodayDateShow } from "../../../../../searchBox/TodayDateShow";
import { DayPicker } from "../../../../../searchBox/DayPicker";

type WeatherForcastSearchOneDateProps = {
    children?: Children;
    radioButtonChecked: boolean;
    onChange: (typeName: WeatherForcastEnumType ) => void
};

export const WeatherForcastSearchOneDate: React.FC<WeatherForcastSearchOneDateProps>
    = (props): JSX.Element => {
        return (
            <>
                 
                 <div className='border border-info m-3 mt-5 pt-2 border-2'>
                 <h5 className="">WeatherForcastSearchOneDate</h5>
                     <span>Today: </span>
                    <RadioButton enabled={props.radioButtonChecked}
                    onChange= {props.onChange}
                    motherName={WeatherForcastEnumType.WeatherForcastSearchOneDate}
                    />
                    <TodayDateShow/>
                    <DayPicker/>
                    
                    {/* <DatePicker></DatePicker>
                    <SelectedDate></SelectedDate> */}
                    
                </div>
             
                {props.children}
            </>
        );
    }

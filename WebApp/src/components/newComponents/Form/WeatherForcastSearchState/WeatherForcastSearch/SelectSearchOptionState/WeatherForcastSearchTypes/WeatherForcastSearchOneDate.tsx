import { RadioButton } from '../../../../../searchBox/RadioButton';
import { Children } from '../../../../compTypes';
import { WeatherForcastEnumType } from '../SelectSearchOptionState';
import { TodayDateShow } from '../../../../../searchBox/TodayDateShow';
import { DayPicker } from '../../../../../searchBox/DayPicker';
import { myDate } from '../../../../../../../communication/apiTypes';

type WeatherForcastSearchOneDateProps = {
  children?: JSX.Element | JSX.Element[];
  radioButtonChecked: boolean;
  onChange: (typeName: WeatherForcastEnumType) => void;
  choiceDate: (date: myDate) => void;
};

export const WeatherForcastSearchOneDate: React.FC<
  WeatherForcastSearchOneDateProps
> = (props): JSX.Element => {
  return (
    <>
      <div className="m-1">
        <span>Today: </span>
        <RadioButton
          enabled={props.radioButtonChecked}
          onChange={props.onChange}
          motherName={WeatherForcastEnumType.WeatherForcastSearchOneDate}
        />
        <TodayDateShow />
        <DayPicker choiceDate={props.choiceDate} />
      </div>

      {props?.children}
    </>
  );
};

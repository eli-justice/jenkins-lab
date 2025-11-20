import React from "react";
import Select, { components } from "react-select";
import Image from "next/image";
import momo from "@/app/assets/images/momo.png";

const CustomOption = (props) => (
  <components.Option {...props}>
    <div className="flex items-center gap-2">
        <Image
            src={props.data.image || momo}
            alt={props.data.label}
            width={34}
            height={34}
            className="w-6 h-6 object-contain"
        />
      <span>{props.data.label}</span>
    </div>
  </components.Option>
);

const CustomSingleValue = (props) => (
  <components.SingleValue {...props}>
    <div className="flex items-center gap-2">
        <Image
            src={props.data.image || momo}
            alt={props.data.label}
            width={34}
            height={34}
            className="w-5 h-5 object-contain"
        />
      <span>{props.data.label}</span>
    </div>
  </components.SingleValue>
);

function NetworkSelect({id, options, value, onChange, error, isLoading = false, }) {
  return (
    <div>
      <Select
          id={id}
        options={options}
        value={value}
        onChange={onChange}
          isLoading={isLoading}
        placeholder="Select network"
        components={{
            Option: CustomOption,
            SingleValue:CustomSingleValue,
        }}
        styles={{
          control: (base) => ({
            ...base,
            borderRadius: "8px",
            padding: "2px",
            borderColor: error? "#fb2c36" : "#e5e7eb",
            boxShadow: "none",
          }),
        }}
      />
    </div>
  );
}

export default NetworkSelect;

import React from 'react';
import classes from './MyStd.module.css';

const MyStd = React.forwardRef((props, ref) => {
    return (
        <input ref={ref} className={classes.myInput} {...props}/>
    );
});

export default MyStd;
function fn_col_drilldown(id, xAxisData) {

    var chartDom = document.getElementById(id);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        xAxis: {
            data: ['01 qui', 'Fruits', 'Cars']
        },
        //xAxis: xAxisData,
        yAxis: {},
        dataGroupId: '',
        animationDurationUpdate: 500,
        series: {
            type: 'bar',
            id: 'sales',
            data: [
                {
                    value: 50,
                    groupId: '01 qui',
                    periodo: '2024-02-01',
                    label: {
                        show: true
                    }
                },
                {
                    value: 2,
                    groupId: 'fruits',
                    label: {
                        show: true
                    }
                },
                {
                    value: 4,
                    groupId: 'cars',
                    label: {
                        show: true
                    }
                }
            ],
            universalTransition: {
                enabled: true,
                divideShape: 'clone'
            }
        }
    };
    const drilldownData = [
        {
            dataGroupId: '01 qui',
            data: [
                ['Cats', 4],
                ['Dogs', 2],
                ['Cows', 1],
                ['Sheep', 2],
                ['Pigs', 1]
            ]
        },
        {
            dataGroupId: 'fruits',
            data: [
                ['Apples', 4],
                ['Oranges', 2]
            ]
        },
        {
            dataGroupId: 'cars',
            data: [
                ['Toyota', 4],
                ['Opel', 2],
                ['Volkswagen', 2]
            ]
        }
    ];
    myChart.on('click', function (event) {
        if (event.data) {
            console.log(event.data);
            var subData = drilldownData.find(function (data) {
                //alert('ee');
                return data.dataGroupId === event.data.groupId;
            });
            if (!subData) {
                return;
            }
            myChart.setOption({
                xAxis: {
                    data: subData.data.map(function (item) {
                        return item[0];
                    })
                },
                series: {
                    type: 'bar',
                    id: 'sales',
                    dataGroupId: subData.dataGroupId,
                    label: { show: true },
                    data: subData.data.map(function (item) {
                        return item[1];
                    }),
                    universalTransition: {
                        enabled: true,
                        divideShape: 'clone'
                    }
                },
                graphic: [
                    {
                        type: 'text',
                        left: 50,
                        top: 20,
                        style: {
                            text: 'Back',
                            fontSize: 18
                        },
                        onclick: function () {
                            myChart.setOption(option);
                        }
                    }
                ]
            });
        }
    });


}


function fn_col_2_barras(id, xAxis_data, seriesName1, seriesName2, series_data1, series_data2, decimais) {

    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);
    xAxis_data = JSON.parse(xAxis_data);
    series_data1 = JSON.parse(series_data1);
    series_data2 = JSON.parse(series_data2);

    var option = {
        title: {
            text: ''
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        //legend: { show: true },
        legend: {
            show: true,
            data: xAxis_data,
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        grid: {
            left: '0%',
            right: '0%',
            top: '15%',
            bottom: '15%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        xAxis: {
            type: 'category',
            data: [],
            axisTick: { show: true }
        },
        yAxis: {
            show: false
        },
        series: [
            {
                name: seriesName1,
                type: 'bar',
                label: {
                    show: true,
                    position: 'insideLeft',
                    formatter: function (params) {
                        var val = parseFloat(params.value);
                        return val.toLocaleString('pt-br', { minimumFractionDigits: decimais, maximumFractionDigits: decimais });
                    }
                },
                data: series_data1
            },
            {
                name: seriesName2,
                type: 'bar',
                label: {
                    show: true,
                    position: 'insideLeft',
                    formatter: function (params) {
                        var val = parseFloat(params.value);
                        return val.toLocaleString('pt-br', { minimumFractionDigits: decimais, maximumFractionDigits: decimais });
                    }
                },
                data: series_data2
            }
        ]
    };

    myChart.setOption(option);
}




function fn_col_realizado_meta(id, realizado, meta) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    let textoTitulo = '';
    let porcentagem = 0;

    if (realizado < meta) {
        porcentagem = (100 * realizado) / meta;
        textoTitulo = `Realizado ${porcentagem.toFixed(2)}% da meta`;
    } else {
        porcentagem = ((realizado / meta) - 1) * 100;
        textoTitulo = `Meta atingida ${porcentagem.toFixed(2)}%`;
    }

    // Specify the configuration items and data for the chart
    var option = {
        grid: {
            left: '0%',
            right: '0%',
            top: '15%',
            bottom: '15%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        title: {
            text: textoTitulo,
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: true,
            data: ['Realizado', 'Meta'],
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            type: 'category',
            data: [],
            axisTick: { show: true }
        },
        yAxis: [
            {
                show: false,
                type: 'value'
            }
        ],
        series: [
            {
                name: "Realizado",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                },
                type: 'bar',
                data: [realizado],
                itemStyle: {
                    color: realizado > meta ? 'darkgreen' : 'darkred'
                }
            },
            {
                name: "Meta",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-br');
                    }
                },
                type: 'bar',
                data: [meta],
                itemStyle: {
                    color: 'rgb(119, 152, 191)'
                }
            }
        ]
    };

    // Display the chart using the configuration items and data just specified.
    myChart.setOption(option);
}

function fn_col_realizado_meta_tendencia(id, realizado, meta, tendencia) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);
    // Specify the configuration items and data for the chart
    var option = {
        grid: {
            left: '0%',
            right: '0%',
            top: '15%',
            bottom: '15%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        title: {
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: true,
            data: ['Realizado', 'Meta', 'Tendência'],
            //top: "60%",
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            type: 'category',
            data: [],
            axisTick: { show: true }
        },
        yAxis: [
            {
                show: false,
                type: 'value'
            }
        ],
        series: [
            {
                name: "Realizado",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: [parseFloat(realizado)]
            },
            {
                name: "Meta",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: [parseFloat(meta)]
            },
            {
                name: "Tendência",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: [parseFloat(tendencia)]
            }
        ]
    };

    // Display the chart using the configuration items and data just specified.
    myChart.setOption(option);
}

function fn_col_periodos(id, seriesName, periodo, valores) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    // Display the chart using the configuration items and data just specified.
    myChart.setOption({
        grid: {
            left: '1%',
            right: '0.5%',
            containLabel: false
        },
        title: {
            text: '',
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: { show: true },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: false,
            data: [],
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            show: true,
            type: 'category',
            data: periodo,
            axisLabel: {
                color: 'gray',
                rotate: 45,
                fontStyle: "normal",
                fontWeight: "normal"
            }
        },
        yAxis: {
            show: false
        },
        series: [
            {
                name: seriesName,
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 15,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: valores
            }
        ]
    });
}

function fn_col_periodos_decimais(id, seriesName, periodo, valores, decimais) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    // Display the chart using the configuration items and data just specified.
    myChart.setOption({
        grid: {
            left: '0%',
            right: '0%',
            top: '10%',
            bottom: '25%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        title: {
            text: '',
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: {
            show: false,
            label: {
                formatter: function (valor) {
                    if (decimais) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 2,
                            maximumFractionDigits: 2
                        });
                    } else {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                }
            }
        },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: false,
            data: [],
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            show: true,
            type: 'category',
            data: periodo,
            axisLabel: {
                color: 'gray',
                rotate: 45,
                fontStyle: "normal",
                fontWeight: "normal"
            }
        },
        yAxis: {
            show: false
        },
        series: [
            {
                name: seriesName,
                label: {
                    show: true,
                    color: 'gray',
                    precision: 0,
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    distance: 15,
                    formatter: function (valor) {
                        if (decimais) {
                            return valor.value.toLocaleString('pt-BR', {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            });
                        } else {
                            return valor.value.toLocaleString('pt-BR', {
                                minimumFractionDigits: 0,
                                maximumFractionDigits: 0
                            });
                        }
                    }
                },
                type: 'bar',
                data: valores
            }
        ]
    });
}

function fn_col_faturamentos_acumulados(id, anos, lojas, valoresMenos2, valoresMenos1, valoresAtuais) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    // Display the chart using the configuration items and data just specified.
    myChart.setOption({
        grid: {
            left: '0%',
            right: '0%',
            top: '10%',
            bottom: '15%',
            height: "auto",
            width: "98%",
            containLabel: false
        },
        title: {
            text: '',
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: true,
            data: lojas,
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            show: true,
            type: 'category',
            data: lojas,
            axisLabel: {
                color: 'gray',
                rotate: 0,
                fontStyle: "normal",
                fontWeight: "normal"
            }
        },
        yAxis: {
            show: false
        },
        series: [
            {
                name: anos[0].toString(),
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    distance: 10,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: valoresMenos2
            },
            {
                name: anos[1].toString(),
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    distance: 10,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: valoresMenos1
            }, {
                name: anos[2].toString(),
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    distance: 10,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: valoresAtuais
            }
        ]
    });
}

function fn_col_cafe_loja_pizza(id, periodo, cafe, loja) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    // Display the chart using the configuration items and data just specified.
    myChart.setOption({
        grid: {
            left: '0%',
            right: '0%',
            top: '10%',
            bottom: '35%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        title: {
            text: '',
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: {
            show: true,
            label: {
                formatter: function (valor) {
                    return valor.value.toLocaleString('pt-BR', {
                        minimumFractionDigits: 0,
                        maximumFractionDigits: 0
                    });
                }
            }
        },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: true,
            data: ["Café", "Loja"],
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            show: true,
            type: 'category',
            data: periodo,
            axisLabel: {
                color: 'gray',
                rotate: 45,
                fontStyle: "normal",
                fontWeight: "normal"
            }
        },
        yAxis: {
            show: false
        },
        series: [
            //{
            //    name: 'Total',
            //    type: 'pie',
            //    center: ['75%', '35%'],
            //    radius: '28%',
            //    z: 100
            //},
            {
                name: "Café",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 15,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: cafe
            },
            {
                name: "Loja",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    rotate: 45,
                    align: 'left',
                    verticalAlign: 'middle',
                    position: 'insideBottom',
                    distance: 15,
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: loja
            }
        ]
    });
}

function fn_col_1(id, realizado) {
    // Initialize the echarts instance based on the prepared dom
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    // Specify the configuration items and data for the chart
    var option = {
        grid: {
            left: '0%',
            right: '0%',
            top: '15%',
            bottom: '15%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        title: {
            text: "",
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: "gray",
                fontWeight: "lighter",
                align: "center"
            },
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "gray"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: true,
            data: ['Realizado'],
            bottom: 0,
            padding: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            type: 'category',
            data: [],
            axisTick: { show: true }
        },
        yAxis: [
            {
                show: false,
                type: 'value'
            }
        ],
        series: [
            {
                name: "Realizado",
                label: {
                    show: true,
                    color: 'gray',
                    position: 'insideBottom',
                    distance: 15,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: function (valor) {
                        return valor.value.toLocaleString('pt-BR', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 0
                        });
                    }
                },
                type: 'bar',
                data: [realizado],
                itemStyle: {
                    color: 'darkgreen'
                }
            }
        ]
    };

    // Display the chart using the configuration items and data just specified.
    myChart.setOption(option);
}

function fn_col_2(id, xAxis, series, decimais, axisLabelRotate) {
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    axisLabelRotate = axisLabelRotate == undefined ? 0 : axisLabelRotate;

    for (var i = 0; i < series.length; i++) {
        series[i].label.formatter = function (valor) {
            return valor.value.toLocaleString('pt-BR', {
                minimumFractionDigits: decimais,
                maximumFractionDigits: decimais
            });
        }
    }

    myChart.setOption({
        grid: {
            left: 0,
            right: 1,
            containLabel: false,
            height: 'auto',
            //width: '100%'
        },
        color: [
            '#402b30',
            'green'
        ],
        title: {
            text: ''
        },
        tooltip: {
            show: true,
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            valueFormatter: function (value) {
                return value.toLocaleString('pt-BR');
            }
        },
        toolbox: {
            show: false
        },
        legend: {
            show: true,
            //bottom: 0,
            top: 0,
            textStyle: {
                color: "gray",
                fontStyle: "normal",
                fontWeight: "normal"
            }
        },
        xAxis: {
            show: true,
            type: 'category',
            data: xAxis,
            axisTick: {
                show: true
            },
            axisLabel: {
                color: 'gray',
                fontStyle: "normal",
                fontWeight: "normal",
                rotate: axisLabelRotate
            }
        },
        yAxis: {
            show: false
        },
        series: series
    });


}

function fn_col_line(id, xAxis, series) {
    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    for (var i = 0; i < series.length; i++) {
        series[i].label.formatter = function (valor) {
            return valor.value.toLocaleString('pt-BR', {
                minimumFractionDigits: 0,
                maximumFractionDigits: 0
            });
        }
    }

    myChart.setOption({
        grid: {
            left: '1%',
            right: '0.5%',
            containLabel: false
        },
        title: {
            text: ''
        },
        tooltip: {
            show: true,
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
        },
        toolbox: {
            show: false
        },
        legend: {
            show: true,
            bottom: 0,
            textStyle: {
                color: "gray",
                fontWeight: "lighter"
            }
        },
        xAxis: {
            show: true,
            type: 'category',
            data: xAxis,
            axisTick: {
                show: true
            },
            axisLabel: {
                color: 'gray',
                fontStyle: "normal",
                fontWeight: "normal"
            }
        },
        //yAxis: [
        //    {
        //        show: false,
        //        type: 'value',
        //        name: 'Precipitation',
        //        min: 0,
        //        max: 250,
        //        interval: 50,
        //        axisLabel: {
        //            formatter: '{value} ml'
        //        }
        //    },
        //    {
        //        show: false,
        //        type: 'value',
        //        name: 'Temperature',
        //        min: 0,
        //        max: 25,
        //        interval: 5,
        //        //axisLabel: {
        //        //    formatter: '{value} °C'
        //        //}
        //    }
        //],
        series: series
    });


}

function fn_col_2_custom(id, legend, legend_data, xAxis_data, decimais, series_data1, series_data2, series_data3, series_linha, imagem_legenda, select_ano_ant) {

    let chartTurnover = document.getElementById(id);
    let myChart = echarts.init(chartTurnover);

    var option = {
        grid: {
            left: '0%',
            right: '0%',
            top: '15%',
            bottom: '15%',
            height: "auto",
            width: "100%",
            containLabel: false
        },
        title: {
            text: "",
            textVerticalAlign: "auto",
            textAlign: "auto",
            textStyle: {
                fontSize: 12,
                color: appGlobal.color,
                fontWeight: appGlobal.fontWeight,
                align: "center"
            },
        },
        tooltip: { show: false },
        toolbox: {
            show: false,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            iconStyle: {
                borderColor: "white"
            },
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            show: legend,
            /*data: legend_data,*/
            data: [
                {
                    name: 'Faturamento',
                    icon: 'image://' + imagem_legenda
                    //itemStyle: {
                    //    color: 'rgb(46, 139, 87)'
                    //}
                },
                {
                    name: 'Meta'
                },
                {
                    name: 'Ano Anterior'
                },
                {
                    name: 'Ponto de Equilibrio'
                }
            ],
            top: 5,
            padding: 0,
            textStyle: {
                color: appGlobal.color,
                fontWeight: appGlobal.fontWeight,
            },
            selected: {
                'Faturamento': true, 
                'Meta': true,       
                'Ano Anterior': select_ano_ant,
                'Ponto de Equilibrio': true
            }
        },
        xAxis: {
            type: 'category',
            data: xAxis_data,
            axisTick: { show: true },
            axisLine: {
                lineStyle: {
                    color: appGlobal.color,
                    fontWeight: appGlobal.fontWeight,
                }
            }
        },
        yAxis: [
            {
                show: false,
                type: 'value'
            }
        ],
        series: [
            {
                name: 'Faturamento',
                type: 'bar',
                data: series_data1,
                label: {
                    show: true,
                    color: appGlobal.color,
                    fontWeight: appGlobal.fontWeight,
                    position: 'insideBottom',
                    distance: 5,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: appGlobal.formatterValor
                },
                itemStyle: {
                    color: function (obj) {
                        let dt1 = series_data1[obj.dataIndex];
                        let dt2 = series_data2[obj.dataIndex];
                        return dt1 >= dt2 ? 'rgb(46, 139, 87)' : 'rgb(205, 92, 92)';
                    }
                }
                //itemStyle: {
                //    color: '#3498db'
                //}
            },
            {
                name: 'Meta',
                type: 'bar',
                data: series_data2,
                label: {
                    show: true,
                    color: appGlobal.color,
                    fontWeight: appGlobal.fontWeight,
                    position: 'insideBottom',
                    distance: 20,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: appGlobal.formatterValor
                },
                itemStyle: {
                    color: '#1E90FF'
                }
            },
            {
                name: 'Ano Anterior',
                type: 'bar',
                data: series_data3,
                label: {
                    show: true,
                    color: appGlobal.color,
                    fontWeight: appGlobal.fontWeight,
                    position: 'insideBottom',
                    distance: 5,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: appGlobal.formatterValor
                },
                itemStyle: {
                    color: '#e67e22'
                }
            },
            {
                name: 'Ponto de Equilibrio',
                type: 'line',
                data: series_linha,
                label: {
                    show: true,
                    color: 'black',
                    fontWeight: appGlobal.fontWeight,
                    position: 'insideBottom',
                    distance: 5,
                    align: 'center',
                    verticalAlign: 'bottom',
                    formatter: appGlobal.formatterValor
                },
                itemStyle: {
                    color: '#8B0000'
                }
            }
        ]
    };

    // Display the chart using the configuration items and data just specified.
    myChart.setOption(option);
}